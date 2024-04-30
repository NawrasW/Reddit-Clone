import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Comment, CommentDtoAddUpdate } from '../models/comments';
import { CommentService } from './comment.service';
import { PostService } from '../post/post.service';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-postdetails',
  templateUrl: './postdetails.component.html',
  styleUrls: ['./postdetails.component.css']
})
export class PostdetailsComponent implements OnInit {
  postId!: number;
  post: any;
  comments: Comment[] = [];
  newCommentContent: string = '';
  loggedInUserId: number | null = null;
  updatingComment: Comment | null = null;
  replyToComment: Comment | null = null;
  replySectionOpen: boolean = true;
  newReplyContent: string = '';
  updateMode: boolean = false;
  editingNestedComment: Comment | null = null;
  constructor(
    private route: ActivatedRoute,
    private commentService: CommentService,
    private postService: PostService,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.postId = +params['id'];
      this.loadPostDetails();
      this.loggedInUserId = +this.authService.getUserId()!;
    });
  }

  loadPostDetails(): void {
    this.postService.getPostById(this.postId).subscribe({
      next: (post) => {
        console.log('Post details loaded:', post);
        this.post = post;
        this.loadComments();
      },
      error: (error) => {
        console.error('Error loading post details:', error);
      }
    });
  }

  loadComments(): void {
    this.commentService.getCommentsByPostId(this.postId).subscribe({
      next: (comments) => {
        console.log('Comments loaded:', comments);
        this.comments = comments;
      },
      error: (error) => {
        console.error('Error loading comments:', error);
      }
    });
  }


  async addComment(): Promise<void> {
    try {
      console.log('Attempting to add a new comment...');
      console.log('Raw comment content:', this.newCommentContent);

      // Trim the input value
      const trimmedContent: string = this.newCommentContent.trim();
      console.log('Trimmed comment content:', trimmedContent);

      // Check if the trimmed content is empty
      if (!trimmedContent) {
        console.error('Error adding comment: Comment content is empty.');
        return;
      }

      if (!this.loggedInUserId) {
        throw new Error('User is not logged in.'); // Removed 'Error: ' from the message
      }

      const fullName: string | null = this.authService.getFullName();
      if (!fullName) {
        throw new Error('Full name not found for logged-in user.'); // Removed 'Error: ' from the message
      }

      console.log('Creating new comment DTO...');
      const newCommentDto: CommentDtoAddUpdate = {
        commentId: 0, // This is valid now
        content: trimmedContent,
        createdAt: new Date(), // Current date and time
        authorUserId: this.loggedInUserId,
        postId: this.postId,

      };

      console.log('New comment DTO:', newCommentDto);

      await this.commentService.addComment(newCommentDto).toPromise();

      console.log('Comment added successfully');
      this.newCommentContent = ''; // Clear the input field
      this.loadComments(); // Reload comments after adding new comment
    } catch (error: any) { // Explicitly type 'error' as 'any'
      console.error('Error adding comment:', error.message); // Access error message using error.message
    }
  }

  isCommentAuthor(comment: Comment): boolean {
    return !!this.loggedInUserId && comment.author.userId === this.loggedInUserId;
  }

  startUpdatingComment(comment: Comment): void {
    this.updatingComment = comment;
  }
  cancelUpdateComment(): void {
    this.updatingComment = null;
  }

  startUpdatingNestedComment(comment: Comment): void {
    this.editingNestedComment = comment;
  }

  cancelUpdateNestedComment(): void {
    this.editingNestedComment = null;
  }


  saveUpdatedComment(comment: Comment): void {
    console.log('Updating comment:', comment); // Add console log here
    if (!this.loggedInUserId) {
      console.error('Error: User is not logged in.');
      return;
    }

    // Ensure createdAt is defined
    const createdAt: Date = comment.createdAt ? new Date(comment.createdAt) : new Date();
    const updatedDate: Date = new Date(); // Set updatedDate to the current date and time

    const updatedCommentDto: CommentDtoAddUpdate = {
      commentId: comment.commentId,
      content: comment.content,
      parentCommentId: comment.parentCommentId,
      updatedDate: updatedDate, // Ensure updatedDate is defined
      postId: comment.postId,
      authorUserId: this.loggedInUserId
    };

    this.commentService.updateComment(updatedCommentDto).subscribe(
      () => {
        console.log('Comment updated successfully');
        this.updatingComment = null; // Reset updatingCommentId
        this.loadComments(); // Reload comments after update
      },
      (error) => {
        console.error('Error updating comment:', error);
      }
    );
  }


  recursiveUpdateComments(comments: Comment[]): void {
    for (const comment of comments) {
      console.log('Editing comment:', comment); // Log the editing process
      this.saveUpdatedComment(comment);
      if (comment.replies && comment.replies.length > 0) {
        this.recursiveUpdateComments(comment.replies);
      }
    }
  }


  deleteComment(comment: Comment): void {
    if (!this.loggedInUserId) {
      console.error('Error: User is not logged in.');
      return;
    }

    // Implement the logic to delete the comment
    this.commentService.deleteComment(comment.commentId).subscribe(
      () => {
        console.log('Comment deleted successfully');
        this.loadComments(); // Reload comments after delete
      },
      (error) => {
        console.error('Error deleting comment:', error);
      }
    );
  }


  addReply(comment: Comment): void {
    this.replyToComment = comment; // Set the comment to which a reply will be added
  }
  submitReply(content: string, parentComment: Comment | null): void {
    if (!content || !parentComment) {
      console.error('Cannot add reply: Content is empty or parent comment is not set');
      return;
    }

    const replyDto: CommentDtoAddUpdate = {
      commentId: 0,
      content: content,
      createdAt: new Date(),
      authorUserId: this.loggedInUserId!,
      postId: this.postId,
      parentCommentId: parentComment.commentId
    };

    this.commentService.addComment(replyDto).subscribe(
      () => {
        console.log('Reply added successfully');
        this.loadComments(); // Reload comments after adding the reply
      },
      (error) => {
        console.error('Error adding reply:', error);
      }
    );
  }

  addReplyToComment(comment: Comment): void {
    // Toggle the reply section open/close if the same comment is clicked again
    this.replyToComment = this.replyToComment === comment ? null : comment;
  }


  toggleReplySection(comment: Comment): void {
    // Toggle the reply section for the clicked comment
    this.replyToComment = this.replyToComment === comment ? null : comment;
    this.replySectionOpen = !this.replySectionOpen; // Toggle the reply section open/close
    this.newReplyContent = ''; // Clear the reply input when toggling
  }



  addNestedReply(comment: Comment): void {
    this.replyToComment = this.replyToComment === comment ? null : comment;
  }

  submitNestedReply(content: string, parentComment: Comment | null): void {
    // Check if content is empty or parentComment is null
    if (!content || !parentComment) {
      console.error('Cannot add nested reply: Content is empty or parent comment is not set');
      return;
    }

    // Create a new comment DTO for the nested reply
    const nestedReplyDto: CommentDtoAddUpdate = {
      commentId: 0,
      content: content,
      createdAt: new Date(),
      authorUserId: this.loggedInUserId!,
      postId: this.postId,
      parentCommentId: parentComment.commentId // Set the parentCommentId to link the reply to the parent comment
    };

    // Add the nested reply using the comment service
    this.commentService.addComment(nestedReplyDto).subscribe(
      () => {
        console.log('Nested reply added successfully');
        // Reload comments after adding the nested reply
        this.loadComments();
      },
      (error) => {
        console.error('Error adding nested reply:', error);
      }
    );
  }







  likeComment(comment: Comment): void {
    if (!this.authService.isLogged()) {
      console.error('User is not logged in.');
      return;
    }

    const userId = this.authService.getUserId();
    if (!userId) {
      console.error('User ID not found.');
      return;
    }

    // Retrieve liked comments from localStorage or initialize an empty object
    let likedComments = JSON.parse(localStorage.getItem('likedComments') || '{}');

    // Check if the comment has already been liked
    if (likedComments[comment.commentId]) {
      console.error('User has already liked this comment.');
      return;
    }

    // Update the local state to reflect liking the comment
    comment.likeCount++;
    comment.isLikedByCurrentUser = true;

    // Update the liked comments in local storage
    likedComments[comment.commentId] = true;
    localStorage.setItem('likedComments', JSON.stringify(likedComments));

    // Call the likeComment function from the CommentService
    this.commentService.likeComment(comment.commentId).subscribe(
      () => {
        console.log('Comment liked successfully.');
        console.error(comment.likeCount);
      },
      (error) => {
        console.error('Error liking comment:', error);
        // Revert the local changes if there's an error
        comment.likeCount--;
        comment.isLikedByCurrentUser = false;
        // Update the liked comments in local storage again to ensure consistency
        likedComments = JSON.parse(localStorage.getItem('likedComments') || '{}');
        delete likedComments[comment.commentId];
        localStorage.setItem('likedComments', JSON.stringify(likedComments));
      }
    );
  }

  dislikeComment(comment: Comment): void {
    if (!this.authService.isLogged()) {
      console.error('User is not logged in.');
      return;
    }

    const userId = this.authService.getUserId();
    if (!userId) {
      console.error('User ID not found.');
      return;
    }

    // Retrieve liked comments from localStorage or initialize an empty object
    let likedComments = JSON.parse(localStorage.getItem('likedComments') || '{}');

    // Check if the comment has already been disliked
    if (!likedComments[comment.commentId]) {
      console.error('User has not liked this comment.');
      return;
    }

    // Update the local state to reflect disliking the comment
    comment.likeCount--;
    comment.isLikedByCurrentUser = false;

    // Remove the comment from the liked comments in local storage
    delete likedComments[comment.commentId];
    localStorage.setItem('likedComments', JSON.stringify(likedComments));

    // Call the dislikeComment function from the CommentService
    this.commentService.dislikeComment(comment.commentId).subscribe(
      () => {
        console.log('Comment disliked successfully.');
      },
      (error) => {
        console.error('Error disliking comment:', error);
        // Revert the local changes if there's an error
        comment.likeCount++;
        comment.isLikedByCurrentUser = true;
        // Update the liked comments in local storage again to ensure consistency
        likedComments = JSON.parse(localStorage.getItem('likedComments') || '{}');
        likedComments[comment.commentId] = true;
        localStorage.setItem('likedComments', JSON.stringify(likedComments));
      }
    );
  }








}
