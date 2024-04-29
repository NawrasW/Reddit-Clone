export class Post {
  postId!: number;
  title!: string;
  content!: string;
  createdAt?: Date;
  author!: { userId: number, name: string };  // Define author as an object with userId and name properties
  likeCount!: number;
  isLikedByCurrentUser!: boolean; 

}
