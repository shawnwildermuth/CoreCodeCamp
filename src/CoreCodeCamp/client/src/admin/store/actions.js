import campActions from "./campActions";
import userActions from "./userActions";
import talkActions from "./talkActions";
import sponsorActions from "./sponsorActions";
import roomActions from "./roomActions";

export default {
  ...campActions,
  ...userActions,
  ...talkActions,
  ...sponsorActions,
  ...roomActions
};