using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emgu_ObjectTracking.Biz
{
    //The main difference between the two methods is that calcOpticalFlowFarneback computes a dense optical flow (motion vector for each pixel),
    //whereas calcOpticalFlowPyrLK computes a sparse optical flow (motion vector for a set of points in the image).
    //Of course, if you don't need to compute motion for every pixel in the image, LK will be faster.
    //Since LK works only on a set of points, you need to specify which points to compute motion vectors for, which is the prevPts argument.
    //This could be a vector containing the location of every pixel in the image (in which case it would be similar to calcOpticalFlowFarneback, but probably slower).
    //However, it's better to specify certain points in the image / video which are of interest and easy to track.
    //These are usually corner like points (which can be found initially using something like goodFeaturesToTrack).

    //https://www.reddit.com/r/computervision/comments/wgez0/opencv_optical_flow_question/
    //https://stackoverflow.com/questions/23422346/can-we-use-lucas-kanade-optical-flowopencv-for-color-based-detection-or-contou
    //motion vector for a set of points in the image

    /// <summary>
    /// Test cơ bản xem method chạy thế nào
    /// </summary>
    class OpticalFlowPyrLK4
    {
        GFTTDetector detector = new GFTTDetector();

        //CalcOpticalFlowPyrLK
        public int levels = 3;
        private Size winSize = new Size(31, 31);
        private double minEigThreshold = 0.01;

        //CornerSubPix
        private Size subPixWinSize = new Size(10, 10);
        public int maxIteration = 20; //Số lần lặp lại lớn nhất
        public double eps = 0.03;


        /// <summary>
        /// X và Y lệch nhau nhỏ hơn eps thì coi như cùng điểm
        /// </summary>
        public double samePointEps = 5;
        /// <summary>
        /// 2 xe là 1 nếu có lớn hơn eps % số điểm ở xe cũ nằm trong xe mới. Ex: 0.1 = 10%
        /// </summary>
        public double sameCarEps = 2;

        private const int MAX_COUNT = 500;
        private bool needToInit = true;
        /// <summary>
        /// Việc nhận dạng sẽ diễn ra sau x frames
        /// </summary>
        private int refreshFrame = 5;

        private Mat gray = new Mat();
        private Mat prevGray = new Mat();
        private UMat image;

        private VectorOfPointF points0 = new VectorOfPointF();
        private VectorOfPointF points1 = null;

        private int PointInCar { get { return _Cars.Sum(c => c.Points.Length); } }
        private int Point0sCount { get { return points0 == null ? 0 : points0.Size; } }
        private int Point1sCount { get { return points1 == null ? 0 : points1.Size; } }

        List<Car> _Cars = new List<Car>();

        ObjectDetector _ObjectDetector = new ObjectDetector(@"Cars.xml");
        int frameCount = 0;
        public void Tracking(UMat img_curr)
        {
            image = img_curr;
            frameCount++;

            CvInvoke.CvtColor(img_curr, gray, ColorConversion.Bgr2Gray);

            if (needToInit)
            {
                //Lấy mẫu
                List<Rectangle> carRecs;
                var newVector = GetFeaturesToTrack(gray, out carRecs);

                //Gộp các point cùng 1 vùng vào 1 xe
                var newCars = GetCars(newVector.ToArray(), carRecs);
                _Cars = GetNewCars(_Cars, newCars);

                points1 = newVector;
                needToInit = false;
            }
            else if (points0.Size > 0)
            {
                VectorOfByte status = new VectorOfByte();
                VectorOfFloat err = new VectorOfFloat();
                if (prevGray.IsEmpty)
                    gray.CopyTo(prevGray);

                //Scope lại sự dịch chuyển của các điểm => Số điểm ít hơn
                //calcOpticalFlowPyrLK(prevGray, gray, points0, points1, status, err, winSize, 3, termcrit, 0, 0.001);
                var termcrit = new MCvTermCriteria();
                CvInvoke.CalcOpticalFlowPyrLK(prevGray, gray, points0, points1, status, err, winSize, levels, termcrit, minEigThreshold: minEigThreshold);

                var tempPoint0 = points0.ToArray().ToList();
                var tempPoint1 = points1.ToArray();

                foreach (var car in _Cars)
                {
                    var newPoints = new List<PointF>();
                    foreach (var point in car.Points)
                    {
                        int index = tempPoint0.IndexOf(point);
                        if (index < 0)
                            continue;
                        //Điểm không còn tracking được nữa => bỏ qua
                        if (status[index] == 0)
                            continue;
                        newPoints.Add(tempPoint1[index]);
                    }
                    car.Points = newPoints.ToArray();
                }

                //size_t i, k;
                int i, k;
                for (i = k = 0; i < tempPoint1.Length; i++)
                {
                    if (status[i] == 0)
                        continue;

                    tempPoint1[k++] = tempPoint1[i];
                }

                //points1.resize(k);
                Array.Resize(ref tempPoint1, k);
                points1 = new VectorOfPointF(tempPoint1);

                if (frameCount % refreshFrame == 0)
                {
                    //Lấy mẫu lại các xe
                    List<Rectangle> carRecs;
                    var newVector = GetFeaturesToTrack(gray, out carRecs);

                    //Lấy các Point mới, Các point cũ đã lấy được từ frame trước (và chuyển sang vị trí mới trùng với vị trí vừa tìm được) thì bỏ qua
                    var newFeatures = GetNewFeatures(img_curr, points1.ToArray().ToList(), newVector.ToArray());
                    points1.Push(newFeatures);

                    //Xem xe nào là xe mới thì thêm vào danh sách
                    List<Car> cars = GetCars(points1.ToArray(), carRecs);
                    var newCars = GetNewCars(_Cars, cars);
                    _Cars.AddRange(newCars);

                    //List<PointF> allCarPoints = new List<PointF>();
                    //foreach(var car in _Cars)
                    //{
                    //    allCarPoints.AddRange(car.Points);
                    //}
                    //points1 = new VectorOfPointF(allCarPoints.ToArray());
                }
            }

            RemoveOutOfRegionCars(img_curr.Size, _Cars);

            Draw(img_curr, points1.ToArray());
            DrawCars(img_curr, _Cars);

            //if( addRemovePt && points1.size() < (size_t)MAX_COUNT )
            //{
            //    vector<Point2f> tmp;
            //    tmp.push_back(point);
            //    cornerSubPix( gray, tmp, winSize, Size(-1,-1), termcrit);
            //    points1.push_back(tmp[0]);
            //    addRemovePt = false;
            //}

            //std::swap(points1, points0);
            var tempPoints = points0;
            points0 = points1;
            points1 = tempPoints;

            //cv::swap(prevGray, gray);
            var tempGray = gray;
            gray = prevGray;
            prevGray = tempGray;
        }

        /// <summary>
        /// Detect các objects có trong frame. Tìm tất cả freatures có trong frame. Chỉ lấy các Features có trong các vùng chứa object
        /// </summary>
        /// <param name="grayFrame"></param>
        /// <param name="objRecs"></param>
        /// <returns></returns>
        private VectorOfPointF GetFeaturesToTrack(IInputOutputArray grayFrame, out List<Rectangle> objRecs)
        {
            //1. Detect các objects có trong frame
            objRecs = _ObjectDetector.DetectGrayImage(grayFrame, minNeighbors: 3, maxsize: 80);

            foreach (Rectangle face in objRecs)
                CvInvoke.Rectangle(image, face, new Bgr(Color.Red).MCvScalar, 2);

            //2. Tìm tất cả freatures có trong frame
            // automatic initialization
            //goodFeaturesToTrack(gray, points1, MAX_COUNT, 0.01, 10, Mat(), 3, 0, 0.04);
            MKeyPoint[] keyPoints = detector.Detect(grayFrame);
            var allPoints = keyPoints.Select(c => c.Point);
            var vectorAllPoints = new VectorOfPointF(allPoints.ToArray());

            //Tính toán lại mẫu cho chính xác hơn => số lượng point ít hơn
            //Vì sau khi tính toán lại => vị trí point mới thay đổi => việc tính point nào nằm trong vùng của object phải tính sau
            //Nếu không sẽ có điểm trước khi tính thì trong object, sau khi tính thì ở ngoài => Mất kiểm soát
            CvInvoke.CornerSubPix(gray, vectorAllPoints, subPixWinSize, new Size(10, 10), new MCvTermCriteria(maxIteration, eps));

            //3. Chỉ lấy các Features có trong các vùng chứa object
            var featureInsideObjects = new List<PointF>();
            var optimizedPoints = vectorAllPoints.ToArray();
            foreach (var objRec in objRecs)
            {
                var carPoints = optimizedPoints.Where(c => c.InsideRectangle(objRec)).ToArray();

                featureInsideObjects.AddRange(carPoints);
            }
            var vectorObjectPoints = new VectorOfPointF(featureInsideObjects.ToArray());

            return vectorObjectPoints;
        }

        private List<Car> GetCars(PointF[] points, List<Rectangle> carRecs)
        {
            var cars = new List<Car>();
            foreach (var objRec in carRecs)
            {
                var car = new Car();
                car.Points = points.Where(c => c.InsideRectangle(objRec)).ToArray();
                car.DetectedRectangle = objRec;
                //car.Name = string.Format("Xe " + Car.Count++);
                cars.Add(car);
            }
            
            List<PointF> test = new List<PointF>();
            foreach(var point in points)
            {
                bool isInCar = cars.Exists(c => c.Points.Any(d => d.X == point.X && d.Y == point.Y));
                if (!isInCar)
                    test.Add(point);
            }


            return cars;

            
        }

        /// <summary>
        /// Tìm trong 2 danh sách, đối tượng nào là mới, đối tượng nào là cũ. Nếu là đối tượng mới thì thêm vào
        /// </summary>
        /// <param name="oldCars"></param>
        /// <param name="newCars"></param>
        /// <param name="eps"></param>
        /// <returns></returns>
        private List<Car> GetNewCars(List<Car> oldCars, List<Car> newCars, double eps = 0.1)
        {
            //Cách 1: Xét 2 PointsBoundary nếu chồng lấn eps% thì coi như là 1
            //Cách 2: Trên mỗi xe đều có các điểm bị dịch chuyển, eps% điểm được chuyển từ Old => New thì đó là cùng 1 xe
            if (oldCars.Count == 0)
            {
                newCars.ForEach(c => c.Name = string.Format("Xe " + ++Car.Count));
                return newCars;
            }

            List<Car> lstActualNewCar = new List<Car>();
            foreach (var car in newCars)
            {
                Car oldCar = oldCars.FirstOrDefault(c => IsSameCar(car, c, sameCarEps));

                if (oldCar == null)
                {
                    //Là xe mới => Thêm vào danh sách
                    lstActualNewCar.Add(car);
                    car.Name = string.Format("Xe " + ++Car.Count);
                }
                else
                {
                    //Nếu là cùng 1 xe thì put các point vừa tìm được vào xe
                    oldCar.Points = oldCar.Points.Concat(car.Points).ToArray();
                }
            }

            //todo: Loại bỏ các điểm không chạy theo đối tượng (Độ dịch chuyển lệch nhiều so với các điểm khác)

            return lstActualNewCar;
        }

        private bool IsSameCar1(Car car1, Car car2, double eps)
        {
            //Trường hợp xe xa dần thì car2 ít point hơn car1
            //Trường hợp xe gần dần thì car2 nhiều point hơn car1

            if (car1.Points.Length == 0)
                return false;

            //Đếm số điểm từ car1 có mặt trong car2
            double samePointCount = 0;
            foreach (var point1 in car1.Points)
            {
                samePointCount += car2.Points.Count(c => c.IsSamePoint(point1, samePointEps));
            }

            double diff = samePointCount / car1.Points.Length;
            return diff > eps;
        }

        private bool IsSameCar(Car car1, Car car2, double eps)
        {
            foreach (var point1 in car1.Points)
            {
                bool isSame = car2.Points.Any(c => c.IsSamePoint(point1, samePointEps));
                if (isSame)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Lấy các Point mới, Các point cũ đã lấy được từ frame trước (và chuyển sang vị trí mới trùng với vị trí vừa tìm được) thì bỏ qua
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="currPoints"></param>
        /// <param name="newPoints"></param>
        /// <returns></returns>
        private PointF[] GetNewFeatures(UMat frame, List<PointF> currPoints, IEnumerable<PointF> newPoints)
        {
            //foreach (var point in currPoints)
            //{
            //    CvInvoke.Circle(frame, point.ToPoint(), 2, new MCvScalar(0, 0, 255));
            //}

            //foreach (var point in newPoints)
            //{
            //    CvInvoke.Circle(frame, point.ToPoint(), 2, new MCvScalar(0, 255, 0));
            //}

            if (currPoints.Count == 0)
            {
                return newPoints.ToArray();
            }

            List<PointF> lstNewPoint = new List<PointF>();
            foreach (var newPoint in newPoints)
            {
                bool hasSamePoint = currPoints.Exists(c => c.IsSamePoint(newPoint, samePointEps));
                if (!hasSamePoint)
                    lstNewPoint.Add(newPoint);
            }

            foreach (var point in lstNewPoint)
            {
                CvInvoke.Circle(frame, point.ToPoint(), 2, new MCvScalar(255, 255, 255));
            }

            return lstNewPoint.ToArray();
        }

        /// <summary>
        /// Nếu xe ra khỏi vùng => Bỏ xe đó đi
        /// </summary>
        private void RemoveOutOfRegionCars(Size frameSize, List<Car> cars)
        {
            var frameRec = new Rectangle(0, 0, frameSize.Width, frameSize.Height);
            for (int i = cars.Count - 1; i >= 0; i--)
            {
                var car = cars[i];

                //Không còn điểm nào hoặc quá ít điểm trong frame
                if (car.Points.Length <= 4)
                {
                    cars.RemoveAt(i);
                    continue;
                }

                //Tâm điểm không còn trong frame
                bool isInsideRectangle = car.PointsCenter.InsideRectangle(frameRec);
                if (!isInsideRectangle)
                    cars.RemoveAt(i);
            }
        }

        public static void DrawCars(UMat frame, List<Car> cars)
        {
            foreach (var car in cars)
            {
                CvInvoke.Rectangle(frame, car.PointsBoundary, new MCvScalar(255, 255, 255), 2);
                CvInvoke.Circle(frame, car.PointsCenter.ToPoint(), 5, new MCvScalar(99, 99, 99));
                CvInvoke.PutText(frame, car.Name, car.PointsCenter.ToPoint(), FontFace.HersheyPlain, 1, new MCvScalar(0, 100, 255), 2);
                foreach (var point in car.Points)
                {
                    CvInvoke.Line(frame, point.ToPoint(), car.PointsCenter.ToPoint(), new MCvScalar(255, 255, 255));
                }
            }
        }

        public static void Draw(UMat frame, IEnumerable<PointF> features)
        {
            foreach (var item in features)
            {
                CvInvoke.Circle(frame, item.ToPoint(), 3, new MCvScalar(0, 255, 0));
            }
        }

        public class Car
        {
            public static int Count = 0;

            public PointF[] Points;
            public Rectangle DetectedRectangle;

            public Rectangle PointsBoundary
            {
                get { return PointExtension.GetBoundary(Points); }
            }
            public PointF PointsCenter
            {
                get { return PointExtension.GetCenterPoint(Points); }
            }

            public string Name;
        }
    }
}
