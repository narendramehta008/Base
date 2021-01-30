import { Component, OnInit } from '@angular/core';
import * as $ from 'jquery';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';

@Component({
  selector: 'app-nav-bootstrap',
  templateUrl: './nav-bootstrap.component.html',
  styleUrls: ['./nav-bootstrap.component.scss']
})
export class NavBootstrapComponent implements OnInit {

  constructor(private authService: AuthenticationService) {

  }
  ngAfterViewInit() {

  }
  ngOnInit() {
  }

}
