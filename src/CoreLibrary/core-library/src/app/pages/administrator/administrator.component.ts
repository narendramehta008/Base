import { Component, OnInit } from '@angular/core';
import { BtspModule } from '../../shared/btsp/btsp.module';
import { CarouselItem } from '@app/shared/btsp/carousel/carousel.component';

@Component({
  selector: 'app-administrator',
  standalone: true,
  imports: [BtspModule],
  templateUrl: './administrator.component.html',
  styleUrl: './administrator.component.scss',
})
export class AdministratorComponent implements OnInit {
  ngOnInit(): void {
    this.images.map((val, index) => {
      this.carouselItems.push({ ImageSrc: val });
    });
  }
  carouselItems: CarouselItem[] = [];
  images = [
    'https://i.pinimg.com/736x/9e/36/f5/9e36f5bfa3d753b380e9adb600b45b5c.jpg',
    'https://i.pinimg.com/736x/e8/eb/8a/e8eb8ace03104ccb6a33f4892110a66a.jpg',
    'https://i.pinimg.com/736x/4b/1c/d1/4b1cd1d0eb61a8e52443a08f6e008d30.jpg',
    // 'https://i.pinimg.com/474x/cd/a2/be/cda2be611e4466e5e6182b627b18f851.jpg',
    // 'https://i.pinimg.com/236x/ef/d3/1a/efd31ac5f95476c4cce63ea716ac511c.jpg',
    // 'https://i.pinimg.com/474x/3e/75/69/3e7569d750a771d5c5ed6ebd1c25aa19.jpg',
    // 'https://i.pinimg.com/474x/5d/92/e9/5d92e99a415012c93e65e529742a902c.jpg',
    // 'https://i.pinimg.com/474x/d4/3b/98/d43b985a3372a50816b6c243f1401358.jpg',
    // 'https://i.pinimg.com/236x/b3/60/ae/b360aefcded25044c9f367c78aea4b07.jpg',
    // 'https://i.pinimg.com/474x/ae/23/4d/ae234ddf0826afc435e6c1388d1cc3b8.jpg',
    // 'https://i.pinimg.com/474x/b8/ac/51/b8ac51e8e5d9de70114f431574907072.jpg',
    // 'https://i.pinimg.com/236x/04/7d/45/047d45fd503ad35dacc6f0fcb632cf99.jpg',
    // 'https://i.pinimg.com/474x/8d/fd/06/8dfd06c0d6d7e108ff6ecf939c0ee1b5.jpg',
    // 'https://i.pinimg.com/474x/c2/ae/fb/c2aefbed78698218736102f618f7dd7d.jpg',
    // 'https://i.pinimg.com/474x/a0/e3/8c/a0e38ce2caa7b7d311a02068bc8b9752.jpg',
    // 'https://i.pinimg.com/236x/f3/34/98/f33498f977fa83129ac66bff0552ca5a.jpg',
    // 'https://i.pinimg.com/474x/d8/04/6a/d8046afc8ea3bf45a4706be0d2050273.jpg',
    // 'https://i.pinimg.com/474x/a4/cd/04/a4cd04ac2648cf40b3676d805e33e8ed.jpg',
    // 'https://i.pinimg.com/474x/97/98/a5/9798a5a8f293bbc39ed3be85550ddde8.jpg',
    // 'https://i.pinimg.com/474x/d2/52/f9/d252f92cfe6d1a8d02226e1dfb29a04a.jpg',
    // 'https://i.pinimg.com/474x/a4/a9/e8/a4a9e83186d5a5573be6c42631139c30.jpg',
    // 'https://i.pinimg.com/474x/be/fe/dd/befeddad10461442c0683fe216fe785a.jpg',
    // 'https://i.pinimg.com/474x/d2/22/11/d22211908f5c427fd009a77413acda66.jpg',
    // 'https://i.pinimg.com/474x/3c/eb/e6/3cebe6963d5a043f2c430165b87e3c7d.jpg',
    // 'https://i.pinimg.com/474x/8d/84/99/8d84996232352416b78fb37daf2d1555.jpg',
    // 'https://i.pinimg.com/474x/d9/7a/e5/d97ae5c6244cba7bd849b3c8efa97a3f.jpg',
    // 'https://i.pinimg.com/474x/5f/31/0d/5f310d2a44033e16b222b50de77be91d.jpg',
    // 'https://i.pinimg.com/474x/88/d5/e4/88d5e4e166a2f46a34d806406b8d5e15.jpg',
    // 'https://i.pinimg.com/474x/b7/d0/25/b7d025d414730c1863e1d87bdf80169b.jpg',
    // 'https://i.pinimg.com/474x/79/0f/28/790f286a43b349010e94feab08555f14.jpg',
    // 'https://i.pinimg.com/474x/76/6b/e3/766be3f9933387e3b8086d16341a450a.jpg',
  ];
  //Array.from(document.getElementsByTagName('img')).map(a=>a.currentSrc);
}
