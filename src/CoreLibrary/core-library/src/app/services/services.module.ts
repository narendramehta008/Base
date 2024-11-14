import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ServicesRoutingModule } from './services-routing.module';
import { SharedModule } from '@app/shared/shared.module';
import { ServicesComponent } from './services.component';
import { JsonHandlerComponent } from './json-handler/json-handler.component';
import { BtspModule } from '../shared/btsp/btsp.module';
import { LearningComponent } from './learning/learning.component';
import { NgxJsonViewerModule } from 'ngx-json-viewer';

@NgModule({
  declarations: [ServicesComponent, JsonHandlerComponent, LearningComponent],
  imports: [
    CommonModule,
    ServicesRoutingModule,
    SharedModule,
    BtspModule,
    NgxJsonViewerModule,
  ],
})
export class ServicesModule {}
