using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projeto_Backend_IQuirium.Model;

namespace Projeto_Backend_IQuirium.Repository.Mapping
{
    public class StatusFeedbackMapping : IEntityTypeConfiguration<StatusFeedback>
    {
        public void Configure(EntityTypeBuilder<StatusFeedback> builder)
        {
            builder.ToTable("status_feedback");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.Atualizado_em).IsRequired().HasDefaultValue(DateTime.Now);

            builder.HasOne(f => f.Feedback).WithMany().HasForeignKey(f => f.Id_feedback);
        }
    }
}

