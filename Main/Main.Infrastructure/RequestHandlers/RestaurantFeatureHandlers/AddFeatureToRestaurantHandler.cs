using Main.Application.Requests.RestaurantFeatureRequests;
using Main.Infrastructure.Database;

namespace Main.Infrastructure.RequestHandlers.RestaurantFeatureHandlers
{
    internal class AddFeatureToRestaurantHandler(MenuslabDbContext dbContext) : IRequestHandler<AddFeatureToRestaurantRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(AddFeatureToRestaurantRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
    internal class DeleteRestaurantFeatureHandler(MenuslabDbContext dbContext) : IRequestHandler<DeleteRestaurantFeatureRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(DeleteRestaurantFeatureRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }

    internal class FetchRestaurantFeatureByRestaurantHandler(MenuslabDbContext dbContext) : IRequestHandler<FetchRestaurantFeatureByRestaurantRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(FetchRestaurantFeatureByRestaurantRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
    internal class FetchRestaurantFeatureHandler(MenuslabDbContext dbContext) : IRequestHandler<FetchRestaurantFeatureRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(FetchRestaurantFeatureRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
    internal class RemoveFeatureFromRestaurantHandler(MenuslabDbContext dbContext) : IRequestHandler<RemoveFeatureFromRestaurantRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(RemoveFeatureFromRestaurantRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
    internal class UpsertRestaurantFeatureHandler(MenuslabDbContext dbContext) : IRequestHandler<UpsertRestaurantFeatureRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(UpsertRestaurantFeatureRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
