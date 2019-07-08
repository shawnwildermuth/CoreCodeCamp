import campActions from "./campActions";
import userActions from "./userActions";
import talkActions from "./talkActions";
import sponsorActions from "./sponsorActions";

export default {
  ...campActions,
  ...userActions,
  ...talkActions,
  ...sponsorActions
};