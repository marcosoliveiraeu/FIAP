namespace FiapCloudGames.Infrastructure.Data
{
    using FiapCloudGames.Domain.Entities;
    using Microsoft.EntityFrameworkCore;

    public class DbContextFCG : DbContext
    {
        public DbContextFCG(DbContextOptions<DbContextFCG> options) : base(options) { }

        // DbSets
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Jogo> Jogos { get; set; }
        public DbSet<JogosUsuario> JogosUsuarios { get; set; }

        public DbSet<Promocao> Promocoes { get; set; }

        public DbSet<PromocaoJogo> PromocaoJogos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mapear enums como string
            modelBuilder.Entity<Usuario>()
                .Property(e => e.Perfil)
                .HasConversion<string>();

            modelBuilder.Entity<Jogo>()
                .Property(e => e.Categoria)
                .HasConversion<string>();

            modelBuilder.Entity<Promocao>()
                .Property(e => e.Status)
                .HasConversion<string>();
        }
    }

}
