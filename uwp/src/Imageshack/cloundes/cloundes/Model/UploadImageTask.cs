// lindexi
// 16:34

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace BitStamp.Model
{
    public abstract class UploadImageTask
    {
        public UploadImageTask(StorageFile file)
        {
            File = file;
            Scale = -1;
            Width = -1;
            Height = -1;
        }

        public UploadImageTask(UploadImageTask uploadImageTask)
        {
            File = uploadImageTask.File;
            Scale = uploadImageTask.Scale;
            Width = uploadImageTask.Width;
            Height = uploadImageTask.Height;
            Name = uploadImageTask.Name;
            Guid = uploadImageTask.Guid;
        }

        public Guid Guid
        {
            set;
            get;
        }

        public StorageFile File
        {
            set;
            get;
        }

        public string Name
        {
            set;
            get;
        }

        public EventHandler<bool> OnUploaded
        {
            set;
            get;
        }

        public string Url
        {
            set;
            get;
        }

        public double Scale
        {
            set;
            get;
        }

        public double Width
        {
            set;
            get;
        }

        public double Height
        {
            set;
            get;
        }

        public abstract void UploadImage();
    }
}