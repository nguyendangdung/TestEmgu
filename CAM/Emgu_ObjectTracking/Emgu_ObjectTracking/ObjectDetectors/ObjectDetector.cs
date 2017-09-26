using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emgu_ObjectTracking.Biz
{
    class ObjectDetector : IDisposable
    {
        private CascadeClassifier _face;
        public ObjectDetector(string cascadeFile)
        {
            //@"C:\Users\Nguyen Truong Giang\Downloads\vehicle_detection_haarcascades-master\vehicle_detection_haarcascades-master\cars.xml"

            //Read the HaarCascade objects
            _face = new CascadeClassifier(cascadeFile);
        }

        public List<Rectangle> Detect(IInputOutputArray image,
                double scaleFactor = 1.1, int minNeighbors = 1, int size = 30, int? maxsize = null)
        {
            List<Rectangle> faces = new List<Rectangle>();
            Size maxSize = maxsize == null ? Size.Empty : new Size(maxsize.Value, maxsize.Value);

            //Read the HaarCascade objects
            using (UMat ugray = new UMat())
            {
                CvInvoke.CvtColor(image, ugray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);

                //normalizes brightness and increases contrast of the image
                //CvInvoke.EqualizeHist(ugray, ugray);

                //Detect the faces  from the gray scale image and store the locations as rectangle
                //The first dimensional is the channel
                //The second dimension is the index of the rectangle in the specific channel                     
                Rectangle[] facesDetected = _face.DetectMultiScale(
                   ugray,
                   scaleFactor: 1.1, //The factor by which the search window is scaled between the subsequent scans, for example, 1.1 means increasing window by 10%
                   minNeighbors: 1, //Minimum number (minus 1) of neighbor rectangles that makes up an object. Use 3 for default.
                   minSize: new Size(size, size),
                   maxSize: maxSize);

                faces.AddRange(facesDetected);
            }

            foreach (Rectangle face in faces)
                CvInvoke.Rectangle(image, face, new Bgr(Color.Red).MCvScalar, 2);
            return faces;

            //foreach (Rectangle eye in eyes)
            //    CvInvoke.Rectangle(image, eye, new Bgr(Color.Blue).MCvScalar, 2);

            ////display the image 
            //using (InputArray iaImage = image.GetInputArray())
            //    ImageViewer.Show(image, String.Format(
            //       "Completed face and eye detection using {0} in {1} milliseconds",
            //       (iaImage.Kind == InputArray.Type.CudaGpuMat && CudaInvoke.HasCuda) ? "CUDA" :
            //       (iaImage.IsUMat && CvInvoke.UseOpenCL) ? "OpenCL"
            //       : "CPU",
            //       detectionTime));

            //foreach (Rectangle face in faces)
            //    CvInvoke.Rectangle(image, face, new Bgr(Color.Red).MCvScalar, 2);
        }

        public List<Rectangle> DetectGrayImage(IInputOutputArray ugray,
                double scaleFactor = 1.1, int minNeighbors = 1, int size = 30, int? maxsize = null)
        {
            List<Rectangle> faces = new List<Rectangle>();
            Size maxSize = maxsize == null ? Size.Empty : new Size(maxsize.Value, maxsize.Value);

            //normalizes brightness and increases contrast of the image
            //CvInvoke.EqualizeHist(ugray, ugray);

            //Detect the faces  from the gray scale image and store the locations as rectangle
            //The first dimensional is the channel
            //The second dimension is the index of the rectangle in the specific channel                     
            Rectangle[] facesDetected = _face.DetectMultiScale(
               ugray,
               scaleFactor: 1.1, //The factor by which the search window is scaled between the subsequent scans, for example, 1.1 means increasing window by 10%
               minNeighbors: 1, //Minimum number (minus 1) of neighbor rectangles that makes up an object. Use 3 for default.
               minSize: new Size(size, size),
               maxSize: maxSize);

            faces.AddRange(facesDetected);

            return faces;
        }

        public void Dispose()
        {
            if (_face != null)
                _face.Dispose();
        }
    }
}
