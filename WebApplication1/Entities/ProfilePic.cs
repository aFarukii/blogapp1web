using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Entities
{
    public class ProfilePic
    {
        [Key]
        public int Id { get; set; }

        // Store the image data as a byte array
        public byte[] Content { get; set; }

        public string FileName { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime UploadTime { get; set; } = DateTime.Now;
    }
}
