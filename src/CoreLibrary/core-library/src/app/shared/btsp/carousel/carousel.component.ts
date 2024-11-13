import { Component, Input } from '@angular/core';

@Component({
  selector: 'btsp-carousel',
  templateUrl: './carousel.component.html',
  styleUrl: './carousel.component.scss',
})
export class CarouselComponent {
  @Input() items: CarouselItem[] = [];
}

export interface CarouselItem {
  ImageSrc: string;
  Title?: string;
  Details?: string;
}
