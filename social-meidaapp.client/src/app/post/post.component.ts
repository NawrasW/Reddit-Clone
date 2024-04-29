import { Component, Input, OnInit } from '@angular/core';
import { PostService } from './post.service';
import { HttpErrorResponse } from '@angular/common/http';
import { Post } from '../models/posts';
import { formatDate } from '@angular/common';
import 'boxicons'
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.css']
})
export class PostComponent implements OnInit {
  @Input() searchText: string = '';
  @Input() posts: Post[] = [];
  filteredPosts: Post[] = []; // Add filteredPosts array
  showSkeleton: boolean = true;
  skeletonPosts: any[] = Array(5).fill({}); // Array for skeleton loaders, adjust the length as needed

  constructor(private postService: PostService, private authService: AuthService) { }

  ngOnInit(): void {
    // Simulating delay to fetch posts data
    setTimeout(() => {
      // Fetch actual posts data
      this.postService.getAllPosts().subscribe(
        (posts: Post[]) => {
          this.filteredPosts = posts.map(post => ({
            ...post,
            createdAt: post.createdAt ? new Date(post.createdAt) : new Date()
          }));
          // Hide skeleton loader
          this.showSkeleton = false;
        },
        (error: HttpErrorResponse) => {
          console.error('Error:', error);
        }
      );
    }, 1000); // Adjust the delay as per your requirement
  }

  formatDateAgo(date: Date | undefined): string {
    if (!date) {
      return '';
    }

    const currentDate = new Date();
    const diff = currentDate.getTime() - date.getTime();
    const seconds = Math.floor(diff / 1000);
    const minutes = Math.floor(seconds / 60);
    const hours = Math.floor(minutes / 60);
    const days = Math.floor(hours / 24);
    const months = Math.floor(days / 30); // Calculate months based on 30 days

    if (months > 0) {
      return `${months} month${months > 1 ? 's' : ''} ago`;
    } else if (days > 0) {
      return `${days} day${days > 1 ? 's' : ''} ago`;
    } else if (hours > 0) {
      return `${hours} hour${hours > 1 ? 's' : ''} ago`;
    } else if (minutes > 0) {
      return `${minutes} minute${minutes > 1 ? 's' : ''} ago`;
    } else {
      return 'Just now';
    }
  }


  applySearchFilter(searchText: string): void {
    if (!searchText.trim()) {
      this.filteredPosts = [...this.posts]; // Reset to all posts if search text is empty
    } else {
      this.filteredPosts = this.posts.filter(post =>
        post.title.toLowerCase().includes(searchText.toLowerCase())
      );
    }
  }

  likePost(post: Post): void {
    if (!this.authService.isLogged()) {
      console.error('User is not logged in.');
      return;
    }

    const userId = this.authService.getUserId();
    if (!userId) {
      console.error('User ID not found.');
      return;
    }

    const likedPosts = JSON.parse(localStorage.getItem('likedPosts') || '{}');
    if (likedPosts[post.postId]) {
      console.error('User has already liked this post.');
      return;
    }

    // Update the local state to reflect liking the post
    post.likeCount++;
    post.isLikedByCurrentUser = true;

    // Update the liked posts in local storage
    likedPosts[post.postId] = true;
    localStorage.setItem('likedPosts', JSON.stringify(likedPosts));

    // Call the likePost function from the PostService
    this.postService.likePost(post.postId).subscribe(
      () => {
        console.log('Post liked successfully.');
        console.log(post.likeCount);
      },
      (error) => {
        console.error('Error liking post:', error);
        // Revert the local changes if there's an error
        post.likeCount--;
        post.isLikedByCurrentUser = false;
        delete likedPosts[post.postId];
        localStorage.setItem('likedPosts', JSON.stringify(likedPosts));
      }
    );
  }

  dislikePost(post: Post): void {
    if (!this.authService.isLogged()) {
      console.error('User is not logged in.');
      return;
    }

    const userId = this.authService.getUserId();
    if (!userId) {
      console.error('User ID not found.');
      return;
    }

    const likedPosts = JSON.parse(localStorage.getItem('likedPosts') || '{}');
    if (!likedPosts[post.postId]) {
      console.error('User has not liked this post.');
      return;
    }

    // Update the local state to reflect disliking the post
    post.likeCount--;
    post.isLikedByCurrentUser = false;

    // Remove the post from the liked posts in local storage
    delete likedPosts[post.postId];
    localStorage.setItem('likedPosts', JSON.stringify(likedPosts));

    // Call the dislikePost function from the PostService
    this.postService.dislikePost(post.postId).subscribe(
      () => {
        console.log('Post disliked successfully.');
      },
      (error) => {
        console.error('Error disliking post:', error);
        // Revert the local changes if there's an error
        post.likeCount++;
        post.isLikedByCurrentUser = true;
        likedPosts[post.postId] = true;
        localStorage.setItem('likedPosts', JSON.stringify(likedPosts));
      }
    );
  }




}
