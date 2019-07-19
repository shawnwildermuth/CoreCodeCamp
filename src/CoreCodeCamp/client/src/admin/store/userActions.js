import AdminDataService from "../adminDataService";

export default {
  async toggleUserAdmin({ commit }, user) {
    let svc = new AdminDataService();
    var res = await svc.toggleAdmin(user);
    commit("setUserAdmin", { user, value: res.data });
  },
  async toggleUserConfirmation({ commit }, user) {
    let svc = new AdminDataService();
    var res = await svc.toggleConfirmation(user);
    commit("setUserConfirmation", { user, value: res.data });
  }
};