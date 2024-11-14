import {
  AfterContentChecked,
  ChangeDetectorRef,
  Component,
  ContentChild,
  ContentChildren,
  ElementRef,
  Input,
  QueryList,
  SimpleChanges,
  TemplateRef,
} from '@angular/core';

export interface ICarouselTemplate {
  Id: string;
  Indicators: boolean;
  Buttons: boolean;
  OnlyContent: boolean;
}

@Component({
  selector: 'btsp-carousel-item',
  template: ` <ng-content></ng-content>`,
  styles: ``,
})
export class CarouselItemComponent {
  @Input() ImageSrc?: string | undefined;
  @Input() Title?: string;
  @Input() Details?: string;
  @ContentChild('carouselTemplate') Template: TemplateRef<any> | undefined;
}

@Component({
  selector: 'btsp-carousel',
  templateUrl: './carousel.component.html',
  styleUrl: './carousel.component.scss',
})
export class CarouselComponent implements AfterContentChecked {
  @ContentChildren(CarouselItemComponent, { descendants: true }) carouselItems:
    | QueryList<CarouselItemComponent>
    | undefined;
  // @Input() items: CarouselItem[] = [];
  @Input() carousel: ICarouselTemplate = {
    Id: 'carousel' + Date.now(),
    Indicators: true,
    Buttons: true,
    OnlyContent: false,
  };

  constructor(private cdr: ChangeDetectorRef) {}

  ngAfterContentChecked(): void {
    if (this.carouselItems)
      this.carousel.Buttons = this.carousel.Indicators =
        this.carouselItems.length > 1;
  }

  ngOnChanges(changes: SimpleChanges): void {
    for (const inputName in changes) {
      if (inputName != 'carouselItems') continue;
      let items: QueryList<CarouselItemComponent> =
        changes[inputName].currentValue;
      this.carousel.Buttons = this.carousel.Indicators = items.length > 1;
    }
  }
}
