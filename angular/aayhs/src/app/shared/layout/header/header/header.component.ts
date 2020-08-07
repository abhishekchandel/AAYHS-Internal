import { Component, OnInit, Input } from '@angular/core';
import { LocalStorageService } from '../../../../core/services/local-storage.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  @Input() title: string;
  constructor(private localStorageService: LocalStorageService) { }

  ngOnInit(): void {
  }

  logoutSession() {
    this.localStorageService.logout();
  }
}
