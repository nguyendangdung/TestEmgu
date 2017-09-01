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
    class ObjectDetector
    {
        //@"C:\Users\Nguyen Truong Giang\Downloads\vehicle_detection_haarcascades-master\vehicle_detection_haarcascades-master\cars.xml"
        public List<Rectangle> Detect(IInputOutputArray image, string cascadeFile)
        {
            List<Rectangle> faces = new List<Rectangle>();

			//Read the HaarCascade objects
			using (CascadeClassifier face = new CascadeClassifier(cascadeFile))
			using (UMat ugray = new UMat())
	        {
		        CvInvoke.CvtColor(image, ugray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);

		        //normalizes brightness and increases contrast of the image
		        //CvInvoke.EqualizeHist(ugray, ugray);

		        //Detect the faces  from the gray scale image and store the locations as rectangle
		        //The first dimensional is the channel
		        //The second dimension is the index of the rectangle in the specific channel                     
		        Rectangle[] facesDetected = face.DetectMultiScale(
			        ugray,
			        1.1,
			        1,
			        new Size(30, 30),
			        ugray.Size);

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
    }
}
