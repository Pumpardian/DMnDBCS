namespace DMnDBCS.API.Services
{
    public interface IImageService
    {
        Task<string> SaveImage(IFormFile formFile);
        Task DeleteImage(string image);
    }

    public class ImageService(IWebHostEnvironment environment, IHttpContextAccessor contextAccessor) : IImageService
    {
        private readonly IWebHostEnvironment _environment = environment;
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

        public Task DeleteImage(string image)
        {
            var imagePath = Path.Combine(_environment.WebRootPath, "Images");
            var filePath = Path.Combine(imagePath, Path.GetFileName(image));

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            return Task.CompletedTask;
        }

        public async Task<string> SaveImage(IFormFile formFile)
        {
            var imagePath = Path.Combine(_environment.WebRootPath, "Images");
            var host = _contextAccessor.HttpContext!.Request.Host;

            var filename = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
            var filePath = Path.Combine(imagePath, filename);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream, CancellationToken.None);
            }

            return await Task.FromResult($"https://{host}/Images/{filename}");
        }
    }
}
