import { Component, EventEmitter, Output } from '@angular/core';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent {
  @Output() searchChange = new EventEmitter<string>();
  loggedinUser: string | undefined;
  loggedinUserId: number | undefined;

  constructor(private auth: AuthService, private router: Router) { }

  onSearchChange(event: any) {
    const searchText = event.target.value.trim();
    this.searchChange.emit(searchText);
  }

  loggedin() {
    const fullName = this.auth.getFullName();
    const userId = this.auth.getUserId();

    if (fullName && userId) {
      this.loggedinUser = fullName;
      this.loggedinUserId = +userId;
      return true;
    } else {
      return false;
    }
  }

  logout() {
    this.auth.logOut();
    this.router.navigateByUrl('/');
  }
}
