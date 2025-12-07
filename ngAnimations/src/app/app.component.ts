import { transition, trigger, useAnimation } from '@angular/animations';
import { Component } from '@angular/core';
import { bounce, shake, tada } from 'ng-animate';
import { lastValueFrom, timer } from 'rxjs';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css'],
    standalone: true,
    animations: [
    trigger('bounce', [transition(':increment', useAnimation(bounce, {params: {timing: 2}}))]),
    trigger('shake', [transition(':increment', useAnimation(shake, {params: {timing: 4}}))]),
    trigger('tada', [transition(':increment', useAnimation(tada, {params: {timing: 3}}))]),
],
})
export class AppComponent {
  title = 'ngAnimations';

  ngshake: number = 0
  ngbounce: number = 0
  ngtada: number = 0

  turn: boolean = false

  constructor() {
  }

  async animateOnce() {
    this.ngshake++
    await lastValueFrom(timer(4000))
    this.ngbounce++
    await lastValueFrom(timer(1000))
    this.ngtada++
    await lastValueFrom(timer(3000))
  }

  async animateLoop() {
    await this.animateOnce()
    setTimeout(() => {this.animateLoop(), 0})
  }

  async turnIt() {
    this.turn = true
    await lastValueFrom(timer(2000))
    this.turn = false
  }
}
