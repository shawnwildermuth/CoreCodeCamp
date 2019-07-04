import campActions from "./campActions";
import userActions from "./userActions";
import talkActions from "./talkActions";

export default {
  ...campActions,
  ...userActions,
  ...talkActions
};