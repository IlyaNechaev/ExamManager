let addButton = $('.add-vmachine');

let addVMachine = function () {
    let newVMachine = $(`
        <div class="vmachine">
          <input type="text" value=""/>
          <a href="#" class="delete">
            <i class="fa fa-solid fa-trash"></i>
          </a>
        </div>`);

    newVMachine.insertBefore(addButton);
}

let setupVMachinesList = function () {

}

addButton.on('click', addVMachine);