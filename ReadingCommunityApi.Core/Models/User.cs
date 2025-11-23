using System.Collections;
using Microsoft.AspNetCore.Identity;

namespace ReadingCommunityApi.Core.Models;

public class User : IdentityUser<int>
{
    public string Bio { get; private set; }
    public string ProfileImageUrl { get; private set; }
    public DateTime? DateOfBirth { get; private set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<WishlistCollection> WishlistCollections { get; private set; } = new List<WishlistCollection>();
    public ICollection<Review> Reviews { get; private set; } = new List<Review>();
    public ICollection<UserFollow> Followers { get; private set; } = new List<UserFollow>();
    public ICollection<UserFollow> Following { get; private set; } = new List<UserFollow>();
    public ICollection<Community> CommunitiesCreated {get; private set;} = new List<Community>();
    public ICollection<CommunityEventAttendee> CommunityEventAttendees {get; private set;} = new List<CommunityEventAttendee>();
    public ICollection<CommunityMember> CmmunityMembers {get; private set;} = new List<CommunityMember>();
    public ICollection<CommunityPost> CommunityPosts {get; private set;} = new List<CommunityPost>();
    public ICollection<CommunityPostComment> CommunityPostComments {get; private set;} = new List<CommunityPostComment>();
    public ICollection<CommunityPostReaction> CommunityPostReactions {get; private set;} = new List<CommunityPostReaction>();
    public ICollection<ConversationParticipant> ConversationParticipants {get; private set;} = new List<ConversationParticipant>();
    public ICollection<Message> SendedMessage {get; private set;} = new List<Message>();
    public ICollection<MessageReaction> MessageReaction {get; private set;} = new List<MessageReaction>();

    public User(string username, string email)
    {
        this.UserName = username;
        this.Email = email;
        this.CreatedAt = DateTime.UtcNow;
    }

    public void AddReview(Review review)
    {
        this.Reviews.Add(review);
    }
    public User(){}
}