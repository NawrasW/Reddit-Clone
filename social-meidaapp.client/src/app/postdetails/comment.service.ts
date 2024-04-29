import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError } from 'rxjs';
import { Comment, CommentDtoAddUpdate } from '../models/comments';
import { Status } from '../models/status';
import { AuthService } from '../auth.service';

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  private apiUrl = 'https://localhost:7192/api/Comment';

  constructor(private http: HttpClient, private authService: AuthService) { }

  getCommentsByPostId(postId: number): Observable<Comment[]> {
    return this.http.get<Comment[]>(`${this.apiUrl}/getAllCommentsByPostId/${postId}`);
  }

  addComment(comment: CommentDtoAddUpdate): Observable<Status> {
    return this.http.post<Status>(`${this.apiUrl}/addComment`, comment);
  }

  updateComment(comment: CommentDtoAddUpdate): Observable<Status> {
    return this.http.put<Status>(`${this.apiUrl}/UpdateComment`, comment);
  }

  deleteComment(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/deleteComment/${id}`);
  }





  likeComment(commentId: number): Observable<any> {
    const token = this.authService.getToken(); // Obtain token from AuthService
    const userId = this.authService.getUserId(); // Obtain user ID from AuthService

    const requestBody = { userId, token, commentId };

    return this.http.post<any>(`${this.apiUrl}/like/${commentId}`, requestBody).pipe(
      catchError(error => {
        console.error('Error liking comment:', error);
        throw error;
      })
    );
  }

  dislikeComment(commentId: number): Observable<any> {
    const token = this.authService.getToken(); // Obtain token from AuthService
    const userId = this.authService.getUserId(); // Obtain user ID from AuthService

    const requestBody = { userId, token, commentId };

    return this.http.post<any>(`${this.apiUrl}/unlike/${commentId}`, requestBody).pipe(
      catchError(error => {
        console.error('Error disliking comment:', error);
        throw error;
      })
    );
  }
}
