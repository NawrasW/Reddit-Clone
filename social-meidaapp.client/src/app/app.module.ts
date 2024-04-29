import { HttpClientModule } from '@angular/common/http';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { PostComponent } from './post/post.component';
import { TimeAgoPipe } from './time-ago.pipe';
import { HeaderComponent } from './header/header.component';
import { PostdetailsComponent } from './postdetails/postdetails.component';
import { LoginComponent } from './User/login/login.component';
import { RegistrationComponent } from './User/registration/registration.component';

@NgModule({
  declarations: [
    AppComponent,
    PostComponent,
    TimeAgoPipe,
    HeaderComponent,
    PostdetailsComponent,
    LoginComponent,
    RegistrationComponent
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule, FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class AppModule { }
