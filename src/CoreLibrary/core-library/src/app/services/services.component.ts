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
  images: string[] = [];
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
    this.images = new DataSource().getImages();

    let card = [
      new CardComponent(this.utilService).setCardValue({
        title: 'JSON Handler',
        fontClass: 'fg-theme',
        cardClass: 'card-neu',
        media: {
          src: routes[1].data?.['icon'],
        },
        onHoverShowDetails: true,
        redirectUrl: `/services/${routes[1].path}`,
        redirectName: 'Visit',
        text: 'JSON (JavaScript Object Notation) is a lightweight data-interchange format.',
      }),
      new CardComponent(this.utilService).setCardValue({
        title: 'Learning',
        fontClass: 'fg-theme',
        cardClass: 'card-neu',
        media: {
          src: routes[2].data?.['icon'],
        },
        onHoverShowDetails: true,
        redirectUrl: `/services/${routes[2].path}`,
        redirectName: 'Visit',
        text: 'Learning is the process of acquiring new understanding, knowledge, behaviors, skills, values, attitudes, and preferences.',
      }),

      new CardComponent(this.utilService).setCardValue({
        title: 'Pinterest',
        fontClass: 'fg-theme',
        media: {
          src: this.images[2],
        },
        onHoverShowDetails: true,
        redirectUrl: '/services/pin',
        redirectName: 'Visit',
        text: "Looking for creative ideas? Whether you're planning your next big travel adventure, searching for home design concepts, looking for fashion & fitness ...",
      }),
      new CardComponent(this.utilService).setCardValue({
        title: 'Data Structure and Algorithms',
        fontClass: 'fg-theme',
        media: {
          src: this.images[3],
        },
        onHoverShowDetails: true,
        redirectUrl: '/dsa/introduction',
        redirectName: 'Visit',
        text: 'Here we discuss basic cpncepts of DS',
      }),
    ];
    this.dataSource.push(...card);
  }
}
