import { transition, trigger, useAnimation } from '@angular/animations';
import { Component } from '@angular/core';
import { bounce, shake, shakeX, tada } from 'ng-animate';
import { lastValueFrom, timeInterval, timer } from 'rxjs';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css'],
    standalone: true,
    animations: [
      trigger("shake", [transition(":increment", useAnimation(shake, {params: { timing: 2}}))]),
      trigger("bounce", [transition(":increment", useAnimation(bounce, {params: { timing: 4}}))]),
      trigger("tada", [transition(":increment", useAnimation(tada, {params: { timing: 3}}))]),
      
    ],
    
})
export class AppComponent {
  title = 'ngAnimations';

  ngShake: number = 0
  ngBounce: number = 0
  ngTada: number = 0

  css_turn: boolean = false

  constructor() {
  }

  async animateOnce() {
    this.ngShake++
    await lastValueFrom(timer(2000))
    this.ngBounce++
    await lastValueFrom(timer(3000))
    this.ngTada++
    await lastValueFrom(timer(3000))
  }

  async animateLoop() {
    await this.animateOnce()
    setTimeout(() => {this.animateLoop(), 0})
  }

  async turnSquare(){
    this.css_turn = true
    await lastValueFrom(timer(2000))
    this.css_turn = false
  }
}
