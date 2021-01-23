import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavToolbarComponent } from './layout/nav-toolbar/nav-toolbar.component';
import { NavIconToolbarComponent } from './layout/nav-icon-toolbar/nav-icon-toolbar.component';

@NgModule({
  declarations: [
    AppComponent,
    NavToolbarComponent,
    NavIconToolbarComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
