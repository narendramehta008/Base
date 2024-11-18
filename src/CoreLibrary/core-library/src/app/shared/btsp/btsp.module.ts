import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  CarouselComponent,
  CarouselItemComponent,
} from './carousel/carousel.component';
import { CardComponent } from './card/card.component';
import { RouterModule } from '@angular/router';
import { CardContainerComponent } from './card-container/card-container.component';
import { SharedModule } from '../shared.module';
import { FileSaverModule } from 'ngx-filesaver';
import { CardGroupComponent } from './card-group/card-group.component';
import { SummaryComponent } from './summary/summary.component';
import { ColComponent, TableComponent } from './table/table.component';

@NgModule({
  declarations: [
    CarouselComponent,
    CarouselItemComponent,
    CardComponent,
    CardContainerComponent,
    CardGroupComponent,
    SummaryComponent,
    TableComponent,
    ColComponent,
  ],
  imports: [CommonModule, RouterModule, SharedModule, FileSaverModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  exports: [
    CarouselComponent,
    CarouselItemComponent,
    CardComponent,
    CardContainerComponent,
    CardGroupComponent,
    SummaryComponent,
    TableComponent,
    ColComponent,
  ],
})
export class BtspModule {}
