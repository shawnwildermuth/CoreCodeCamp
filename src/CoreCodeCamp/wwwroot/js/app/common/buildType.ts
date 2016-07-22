import { enableProdMode } from '@angular/core';

export function buildType() {
  if (this.process && this.process.env.ASPNETCORE_ENVIRONMENT !== "Development") {
    enableProdMode();
    console.log("Enabling Production Mode");
  } 
};