<template>
  <div role="tabpanel" class="tab-pane fade" id="rooms">
    <table class="table table-condensed table-responsive table-striped">
      <thead>
        <tr>
          <td class="col-md-10">Name</td>
          <td class="col-md-2"></td>
        </tr>
      </thead>
      <tr v-for="room in rooms">
        <td>{{ room.name }}</td>
        <td>
          <button class="btn btn-sm btn-danger" title="Delete" @@click="onDeleteRoom(room)">
            <i class="fa fa-remove"></i>
          </button>
        </td>
      </tr>
    </table>
    <input v-model="newRoom" name="newRoom" />
    <button class="btn btn-success" @@click="onSaveRoom()">
      <i class="fa fa-plus"></i>
    </button>
  </div>
</template>

<script>
export default {};
</script>

<style>
</style>
