import App from "./app";
import JoinView from "./joinView";

function showJoin() {
  let app: App = new App(new JoinView());
  app.bootstrap();
}