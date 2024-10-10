using Main.Application.CustomTypes;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.Database.Entities
{
    internal class PaymentGateway
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string ImplementationName { get; set; }
        public List<Country> Countries { get; set; } = [];
        
    }
}
