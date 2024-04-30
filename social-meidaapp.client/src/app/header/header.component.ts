import { Component, EventEmitter, Output } from '@angular/core';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';
import {
  Dropdown,
  Ripple,
  initTWE,
} from "tw-elements";

initTWE({ Dropdown, Ripple });

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent {
  @Output() searchChange = new EventEmitter<string>();
  loggedinUser: string | undefined;
  loggedinUserId: number | undefined;
  dropdownOpen = false;

  constructor(private auth: AuthService, private router: Router) { }

  onSearchChange(event: any) {
    const searchText = event.target.value.trim();
    this.searchChange.emit(searchText);
  }


  toggleDropdown() {
    this.dropdownOpen = !this.dropdownOpen;
    console.log('Dropdown open:', this.dropdownOpen);
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
