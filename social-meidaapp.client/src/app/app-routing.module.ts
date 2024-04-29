import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PostComponent } from './post/post.component';
import { PostdetailsComponent } from './postdetails/postdetails.component';
import { LoginComponent } from './User/login/login.component';

const routes: Routes = [{ path: '', pathMatch: 'full', redirectTo: '/home' },
  { path: 'home', component: PostComponent },
  { path: 'Replies/:id', component: PostdetailsComponent },
  { path: 'signin', component: LoginComponent } ];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
