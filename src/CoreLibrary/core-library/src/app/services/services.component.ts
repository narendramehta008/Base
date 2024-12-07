import { AfterContentChecked, AfterViewInit, Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataSource } from '@app/core/Data/data-source';
import { CardComponent } from '@app/shared/btsp/card/card.component';
import { UtilsService } from '@app/shared/services/utils.service';
import { routes } from './services-routing.module';

@Component({
  selector: 'app-services',
  templateUrl: './services.component.html',
  styleUrl: './services.component.scss',
})
export class ServicesComponent implements AfterContentChecked {
  dataSource: CardComponent[] = [];
  isChildActivated = false;

  constructor(
    private utilService: UtilsService,
    private active: ActivatedRoute
  ) {}
  ngAfterContentChecked(): void {
    this.isChildActivated = this.active.children.length != 0;
  }
  ngAfterViewInit(): void {}

  //https://similarpng.com/
  ngOnInit(): void {

    let card =   routes.slice(1).map(child=>{
     return new CardComponent(this.utilService).setCardValue({
        title: child.data?.['title'],
        fontClass: 'fg-theme',
        cardClass: 'card-neu',
        media: {
          src: child.data?.['icon'],
        },
        onHoverShowDetails: true,
        redirectUrl: `/services/${child.path}`,
        redirectName: 'Visit',
        text: child.data?.['text'] ?? child.data?.['title'],
      });
    });
   
    this.dataSource.push(...card);
  }
}
