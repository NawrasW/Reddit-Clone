export class Comment {
  commentId!: number;
  content!: string;
  createdAt!: Date;
  updatedDate!: Date
  postId!: number;
  author!: { userId: number, name: string };
  replies?: Comment[] | null;
  parentCommentId?: number | null;
  likeCount!: number;
  isLikedByCurrentUser!: boolean;
}

export class CommentDtoAddUpdate {
  commentId: number | null;
  content: string;
  createdAt?: Date;
  updatedDate?: Date;
  postId: number | null;
  authorUserId: number | null;
  parentCommentId?: number | null;
  // Optional property for parent comment ID

  constructor(
    commentId: number | null,
    content: string,
    postId: number | null,
    authorUserId: number | null,
    parentCommentId?: number | null // Make it optional in the constructor as well
  ) {
    this.commentId = commentId;
    this.content = content;
    this.postId = postId;
    this.authorUserId = authorUserId;
    this.parentCommentId = parentCommentId; // Assign the value passed to the constructor
  }
}

