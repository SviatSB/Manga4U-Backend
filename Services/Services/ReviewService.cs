using DataInfrastructure.Interfaces;
using Services.Interfaces;
using Services.Results.Base;
using Domain.Models;

namespace Services.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMangaService _mangaService;

        public ReviewService(IReviewRepository reviewRepository, IUserRepository userRepository, IMangaService mangaService)
        {
            _reviewRepository = reviewRepository;
            _userRepository = userRepository;
            _mangaService = mangaService;
        }

        public async Task<Result> AddReviewAsync(long userId, string mangaExternalId, int stars, string? text)
        {
            var user = await _userRepository.FindAsync(userId);
            if (user is null) return Result.Failure("User not found");

            var mangaResult = await _mangaService.GetOrAdd(mangaExternalId);
            if (!mangaResult.IsSucceed) return Result.Failure("Manga not found");

            var manga = mangaResult.Value;

            if (await _reviewRepository.UserHasReviewAsync(userId, manga.Id))
                return Result.Failure("User already reviewed this manga");

            var review = new Review
            {
                UserId = userId,
                MangaId = manga.Id,
                Stars = stars,
                Text = text,
            };

            await _reviewRepository.AddAsync(review);
            return Result.Success();
        }

        public async Task<Result> DeleteReviewAsync(long userId, long reviewId)
        {
            var user = await _userRepository.FindAsync(userId);
            if (user is null) return Result.Failure("User not found");

            var review = await _reviewRepository.GetAsync(reviewId);
            if (review is null) return Result.Failure("Review not found");

            var roles = await _userRepository.GetRolesAsync(user);
            if (!roles.Contains("Admin") && review.UserId != userId)
                return Result.Failure("You do not have permission to delete this review");

            await _reviewRepository.DeleteAsync(reviewId);
            return Result.Success();
        }

        public async Task<Result<PagedResult<Review>>> GetReviewsByMangaAsync(string mangaExternalId, int skip, int take)
        {
            var mangaResult = await _mangaService.GetOrAdd(mangaExternalId);
            if (!mangaResult.IsSucceed) return Result<PagedResult<Review>>.Failure("Manga not found");

            var manga = mangaResult.Value;
            var (items, total) = await _reviewRepository.GetReviewsByMangaIdAsync(manga.Id, skip, take);
            var result = new PagedResult<Review>
            {
                TotalCount = total,
                Items = items.ToList()
            };
            return Result<PagedResult<Review>>.Success(result);
        }

        public async Task<double> GetAverageStarsAsync(string mangaExternalId)
        {
            var mangaResult = await _mangaService.GetOrAdd(mangaExternalId);
            if (!mangaResult.IsSucceed) return 0.0;
            var manga = mangaResult.Value;
            return await _reviewRepository.GetAverageStarsAsync(manga.Id);
        }
    }
}
