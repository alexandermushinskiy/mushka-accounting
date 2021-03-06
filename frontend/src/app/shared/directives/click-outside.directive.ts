import { Directive, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';

@Directive({
  selector: '[mkClickOutside]'
})
export class ClickOutsideDirective {
  @Output() clickOutside = new EventEmitter();

  constructor(private elementRef: ElementRef) {
  }

  @HostListener('document:click', ['$event.target'])
  public onClick(targetElement) {
      const clickedInside = this.elementRef.nativeElement.contains(targetElement);
      if (!clickedInside) {
          this.clickOutside.emit();
      }
  }
}
