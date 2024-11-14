import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ServicesRoutingModule } from './services-routing.module';
import { SharedModule } from '@app/shared/shared.module';
import { ServicesComponent } from './services.component';
import { JsonHandlerComponent } from './json-handler/json-handler.component';
import { BtspModule } from '../shared/btsp/btsp.module';
import { LearningComponent } from './learning/learning.component';
import { NgxJsonViewerModule } from 'ngx-json-viewer';
import { DataHandlerComponent } from './data-handler/data-handler.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    ServicesComponent,
    JsonHandlerComponent,
    LearningComponent,
    DataHandlerComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    ServicesRoutingModule,
    SharedModule,
    BtspModule,
    NgxJsonViewerModule,
  ],
})
export class ServicesModule {}
