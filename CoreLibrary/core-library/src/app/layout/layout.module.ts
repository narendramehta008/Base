import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavIconToolbarComponent } from './nav-icon-toolbar/nav-icon-toolbar.component';
import { NavBootstrapComponent } from './nav-bootstrap/nav-bootstrap.component';
import { FooterComponent } from './footer/footer.component';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [NavIconToolbarComponent, NavBootstrapComponent, FooterComponent],
  exports: [NavIconToolbarComponent, NavBootstrapComponent, FooterComponent]

})
export class LayoutModule { }
