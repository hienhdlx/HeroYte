using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCSw.HERO.Core.Domain.Customers;
using NCSw.HERO.Core.Domain.Orders;

namespace NCSw.HERO.Data.Mapping.Customers
{
    /// <summary>
    /// Represents a reward points history mapping configuration
    /// </summary>
    public partial class RewardPointsHistoryMap : NopEntityTypeConfiguration<RewardPointsHistory>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<RewardPointsHistory> builder)
        {
            builder.ToTable(nameof(RewardPointsHistory));
            builder.HasKey(historyEntry => historyEntry.Id);

            builder.Property(historyEntry => historyEntry.UsedAmount).HasColumnType("decimal(18, 4)");

            builder.HasOne(historyEntry => historyEntry.Customer)
                .WithMany()
                .HasForeignKey(historyEntry => historyEntry.CustomerId)
                .IsRequired();

            builder.HasOne(historyEntry => historyEntry.UsedWithOrder)
                .WithOne(order => order.RedeemedRewardPointsEntry)
                .HasForeignKey<Order>(order => order.RewardPointsHistoryEntryId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            base.Configure(builder);
        }

        #endregion
    }
}