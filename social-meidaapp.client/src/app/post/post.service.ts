import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError } from 'rxjs';
import { Post } from '../models/posts';
import { AuthService } from '../auth.service';

import { throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PostService {

  private apiUrl = 'https://localhost:7192/api/Post'; // Update with your API URL

  constructor(private http: HttpClient,private authService: AuthService) { }

  getPostById(id: number): Observable<Post> {
    return this.http.get<Post>(`${this.apiUrl}/getPostById/${id}`);
  }

  getAllPosts(): Observable<Post[]> {
    return this.http.get<Post[]>(`${this.apiUrl}/getAllPosts`);
  }

  addPost(post: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/addPost`, post);
  }

  updatePost(post: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/updatePost`, post);
  }

  deletePost(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/deletePost/${id}`);
  }

  // Method to like a post
  likePost(postId: number): Observable<any> {
    const token = this.authService.getToken(); // Obtain token from AuthService
    const userId = this.authService.getUserId(); // Obtain user ID from AuthService

    const requestBody = { userId, token, postId };

    return this.http.post<any>(`${this.apiUrl}/like/${postId}`, requestBody).pipe(
      catchError(error => {
        console.error('Error liking post:', error);
        throw error;
      })
    );
  }

  dislikePost(postId: number): Observable<any> {
    const token = this.authService.getToken(); // Obtain token from AuthService
    const userId = this.authService.getUserId(); // Obtain user ID from AuthService

    const requestBody = { userId, token, postId };

    return this.http.post<any>(`${this.apiUrl}/unlike/${postId}`, requestBody).pipe(
      catchError(error => {
        console.error('Error disliking post:', error);
        throw error;
      })
    );
  }


}
