using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<BookImage> BookImages { get; set; }
        public DbSet<WishlistCollection> WishlistCollections { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }
        public DbSet<UserFollow> UserFollows { get; set; }
        public DbSet<Community> Communities { get; set; }
        public DbSet<CommunityMember> CommunityMembers { get; set; }
        public DbSet<CommunityPost> CommunityPosts { get; set; }
        public DbSet<CommunityPostComment> CommunityPostComments { get; set; }
        public DbSet<CommunityPostReaction> CommunityPostReactions { get; set; }
        public DbSet<CommunityEvent> CommunityEvents { get; set; }
        public DbSet<CommunityEventAttendee> CommunityEventAttendees { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<ConversationParticipant> ConversationParticipants { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MessageReaction> MessageReactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
            
        // Book - Author (One-to-Many)
        modelBuilder.Entity<Book>()
            .HasOne(b => b.Author)
            .WithMany(a => a.Books)
            .HasForeignKey(b => b.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Book - Categories (Many-to-Many)
        modelBuilder.Entity<Book>()
            .HasMany(b => b.Categories)
            .WithMany(c => c.Books)
            .UsingEntity(j => j.ToTable("BookCategories"));

        // Book - Reviews (One-to-Many)
        modelBuilder.Entity<Book>()
            .HasMany(b => b.Reviews)
            .WithOne(r => r.Book)
            .HasForeignKey(r => r.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        // Book - BookImages (One-to-Many)
        modelBuilder.Entity<Book>()
            .HasMany(b => b.Images)
            .WithOne(bi => bi.Book)
            .HasForeignKey(bi => bi.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        // Book - CommunityEvents (One-to-Many)
        modelBuilder.Entity<Book>()
            .HasMany(b => b.CommunityEvents)
            .WithOne(ce => ce.Book)
            .HasForeignKey(ce => ce.BookId)
            .OnDelete(DeleteBehavior.SetNull);

        // Book - CommunityPosts (One-to-Many)
        modelBuilder.Entity<Book>()
            .HasMany(b => b.CommunityPosts)
            .WithOne(cp => cp.Book)
            .HasForeignKey(cp => cp.BookId)
            .OnDelete(DeleteBehavior.SetNull);

        // Book - Messages (One-to-Many)
        modelBuilder.Entity<Book>()
            .HasMany(b => b.MessagesReference)
            .WithOne(m => m.Book)
            .HasForeignKey(m => m.BookId)
            .OnDelete(DeleteBehavior.SetNull);

        // User - Reviews (One-to-Many)
        modelBuilder.Entity<User>()
            .HasMany(u => u.Reviews)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // User - WishlistCollections (One-to-Many)
        modelBuilder.Entity<User>()
            .HasMany(u => u.WishlistCollections)
            .WithOne(wc => wc.User)
            .HasForeignKey(wc => wc.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // User - Followers (One-to-Many) - Users who follow this user
        modelBuilder.Entity<User>()
            .HasMany(u => u.Followers)
            .WithOne(uf => uf.Following)
            .HasForeignKey(uf => uf.FollowingId)
            .OnDelete(DeleteBehavior.Restrict);

        // User - Following (One-to-Many) - Users this user follows
        modelBuilder.Entity<User>()
            .HasMany(u => u.Following)
            .WithOne(uf => uf.Follower)
            .HasForeignKey(uf => uf.FollowerId)
            .OnDelete(DeleteBehavior.Restrict);

        // UserFollow - Unique constraint (prevent duplicate follows)
        modelBuilder.Entity<UserFollow>()
            .HasIndex(uf => new { uf.FollowerId, uf.FollowingId })
            .IsUnique();

        // User - CommunitiesCreated (One-to-Many)
        modelBuilder.Entity<User>()
            .HasMany(u => u.CommunitiesCreated)
            .WithOne(c => c.Creator)
            .HasForeignKey(c => c.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        // User - CommunityEventAttendees (One-to-Many)
        modelBuilder.Entity<User>()
            .HasMany(u => u.CommunityEventAttendees)
            .WithOne(cea => cea.User)
            .HasForeignKey(cea => cea.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // User - CommunityMembers (One-to-Many)
        modelBuilder.Entity<User>()
            .HasMany(u => u.CmmunityMembers)
            .WithOne(cm => cm.User)
            .HasForeignKey(cm => cm.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // User - CommunityPosts (One-to-Many)
        modelBuilder.Entity<User>()
            .HasMany(u => u.CommunityPosts)
            .WithOne(cp => cp.Author)
            .HasForeignKey(cp => cp.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        // User - CommunityPostComments (One-to-Many)
        modelBuilder.Entity<User>()
            .HasMany(u => u.CommunityPostComments)
            .WithOne(cpc => cpc.Author)
            .HasForeignKey(cpc => cpc.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        // User - CommunityPostReactions (One-to-Many)
        modelBuilder.Entity<User>()
            .HasMany(u => u.CommunityPostReactions)
            .WithOne(cpr => cpr.User)
            .HasForeignKey(cpr => cpr.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // User - ConversationParticipants (One-to-Many)
        modelBuilder.Entity<User>()
            .HasMany(u => u.ConversationParticipants)
            .WithOne(cp => cp.User)
            .HasForeignKey(cp => cp.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // User - SendedMessage (One-to-Many)
        modelBuilder.Entity<User>()
            .HasMany(u => u.SendedMessage)
            .WithOne(m => m.Sender)
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        // User - MessageReaction (One-to-Many)
        modelBuilder.Entity<User>()
            .HasMany(u => u.MessageReaction)
            .WithOne(mr => mr.User)
            .HasForeignKey(mr => mr.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // WishlistCollection - WishlistItems (One-to-Many)
        modelBuilder.Entity<WishlistCollection>()
            .HasMany(wc => wc.Items)
            .WithOne(wi => wi.WishlistCollection)
            .HasForeignKey(wi => wi.WishlistCollectionId)
            .OnDelete(DeleteBehavior.Cascade);

        // WishlistItem - Book (Many-to-One)
        modelBuilder.Entity<WishlistItem>()
            .HasOne(wi => wi.Book)
            .WithMany(b => b.WishlistItems)
            .HasForeignKey(wi => wi.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        // WishlistItem - Unique constraint (prevent duplicate books in same wishlist)
        modelBuilder.Entity<WishlistItem>()
            .HasIndex(wi => new { wi.WishlistCollectionId, wi.BookId })
            .IsUnique();

        // Community - Conversation (One-to-One)
        modelBuilder.Entity<Community>()
            .HasOne(c => c.Conversation)
            .WithOne()
            .HasForeignKey<Community>(c => c.ConversationId)
            .OnDelete(DeleteBehavior.SetNull);

        // Community - Members (One-to-Many)
        modelBuilder.Entity<Community>()
            .HasMany(c => c.Members)
            .WithOne(cm => cm.Community)
            .HasForeignKey(cm => cm.CommunityId)
            .OnDelete(DeleteBehavior.Cascade);

        // Community - Posts (One-to-Many)
        modelBuilder.Entity<Community>()
            .HasMany(c => c.Posts)
            .WithOne(cp => cp.Community)
            .HasForeignKey(cp => cp.CommunityId)
            .OnDelete(DeleteBehavior.Cascade);

        // Community - Events (One-to-Many)
        modelBuilder.Entity<Community>()
            .HasMany(c => c.Events)
            .WithOne(ce => ce.Community)
            .HasForeignKey(ce => ce.CommunityId)
            .OnDelete(DeleteBehavior.Cascade);

        // Community - FocusCategories (Many-to-Many)
        modelBuilder.Entity<Community>()
            .HasMany(c => c.FocusCategories)
            .WithMany()
            .UsingEntity(j => j.ToTable("CommunityCategories"));

        // CommunityMember - Unique constraint (prevent duplicate memberships)
        modelBuilder.Entity<CommunityMember>()
            .HasIndex(cm => new { cm.CommunityId, cm.UserId });

        // CommunityPost - Comments (One-to-Many)
        modelBuilder.Entity<CommunityPost>()
            .HasMany(cp => cp.Comments)
            .WithOne(cpc => cpc.Post)
            .HasForeignKey(cpc => cpc.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        // CommunityPost - Reactions (One-to-Many)
        modelBuilder.Entity<CommunityPost>()
            .HasMany(cp => cp.Reactions)
            .WithOne(cpr => cpr.Post)
            .HasForeignKey(cpr => cpr.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        // CommunityPostComment - ReplyToComment (Self-referencing)
        modelBuilder.Entity<CommunityPostComment>()
            .HasOne(cpc => cpc.ReplyToComment)
            .WithMany()
            .HasForeignKey(cpc => cpc.ReplyToCommentId)
            .OnDelete(DeleteBehavior.Restrict);

        // CommunityPostReaction - Unique constraint (prevent duplicate reactions)
        modelBuilder.Entity<CommunityPostReaction>()
            .HasIndex(cpr => new { cpr.PostId, cpr.UserId, cpr.Emoji })
            .IsUnique();

        // CommunityEvent - Attendees (One-to-Many)
        modelBuilder.Entity<CommunityEvent>()
            .HasMany(ce => ce.Attendees)
            .WithOne(cea => cea.Event)
            .HasForeignKey(cea => cea.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        // CommunityEventAttendee - Unique constraint (prevent duplicate attendances)
        modelBuilder.Entity<CommunityEventAttendee>()
            .HasIndex(cea => new { cea.EventId, cea.UserId })
            .IsUnique();

        // Conversation - Participants (One-to-Many)
        modelBuilder.Entity<Conversation>()
            .HasMany(c => c.Participants)
            .WithOne(cp => cp.Conversation)
            .HasForeignKey(cp => cp.ConversationId)
            .OnDelete(DeleteBehavior.Cascade);

        // Conversation - Messages (One-to-Many)
        modelBuilder.Entity<Conversation>()
            .HasMany(c => c.Messages)
            .WithOne(m => m.Conversation)
            .HasForeignKey(m => m.ConversationId)
            .OnDelete(DeleteBehavior.Cascade);

        // ConversationParticipant - Unique constraint (prevent duplicate participants)
        modelBuilder.Entity<ConversationParticipant>()
            .HasIndex(cp => new { cp.ConversationId, cp.UserId })
            .IsUnique();

        // Message - Reactions (One-to-Many)
        modelBuilder.Entity<Message>()
            .HasMany(m => m.Reactions)
            .WithOne(mr => mr.Message)
            .HasForeignKey(mr => mr.MessageId)
            .OnDelete(DeleteBehavior.Cascade);

        // Message - ReplyToMessage (Self-referencing)
        modelBuilder.Entity<Message>()
            .HasOne(m => m.ReplyToMessage)
            .WithMany()
            .HasForeignKey(m => m.ReplyToMessageId)
            .OnDelete(DeleteBehavior.Restrict);

        // Message - Review (Many-to-One)
        modelBuilder.Entity<Message>()
            .HasOne(m => m.Review)
            .WithMany()
            .HasForeignKey(m => m.ReviewId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);

        // MessageReaction - Unique constraint (prevent duplicate reactions)
        modelBuilder.Entity<MessageReaction>()
            .HasIndex(mr => new { mr.MessageId, mr.UserId, mr.Emoji })
            .IsUnique();

        // Book dimensions
        modelBuilder.Entity<Book>()
            .Property(b => b.Width)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Book>()
            .Property(b => b.Height)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Book>()
            .Property(b => b.Depth)
            .HasPrecision(18, 2);
    
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<IdentityRole<int>>().ToTable("Roles");
        modelBuilder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
        modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
        modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
        modelBuilder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");
        modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
    }
}