<template>
  <v-layout align-start>
    <!--items="categorias" viene de return-->
    <v-flex>
      <v-data-table :headers="headers" :items="categorias" :search="search" class="elevation-1">
        <template v-slot:top>
          <v-toolbar flat color="white">
            <v-toolbar-title>Categoria</v-toolbar-title>
            <v-divider class="mx-4" inset vertical></v-divider>

            <div class="flex-grow-1"></div>

            <!--copiar codigo buqueda-->
            <v-spacer></v-spacer>
            <v-text-field
              class="text-xs-center"
              v-model="search"
              append-icon="search"
              label="Busqueda"
              single-line
              hide-details
            ></v-text-field>
            <v-spacer></v-spacer>

            <!--fin-->

            <v-dialog v-model="dialog" max-width="500px">
              <template v-slot:activator="{ on }">
                <v-btn color="primary" dark class="mb-2" v-on="on">New Item</v-btn>
              </template>
              <v-card>
                <v-card-title>
                  <span class="headline">{{ formTitle }}</span>
                </v-card-title>

                <v-card-text>
                  <v-container grid-list-md>
                    <v-layout wrap>
                      <v-flex xs12 sm12 md12>
                        <v-text-field v-model="nombre" label="Nombre"></v-text-field>
                      </v-flex>
                      <v-flex xs12 sm12 md12>
                        <v-text-field v-model="descripcion" label="Descripcion"></v-text-field>
                      </v-flex>52
                    </v-layout>
                  </v-container>
                </v-card-text>

                <v-card-actions>
                  <v-spacer></v-spacer>
                  <v-btn color="blue darken-1" flat @click.native="close">Cancelar</v-btn>
                  <v-btn color="blue darken-1" flat @click.native="guardar">Guardar</v-btn>
                </v-card-actions>
              </v-card>
            </v-dialog>
          </v-toolbar>
        </template>
        <template v-slot:item.opciones="{ item }">
          <v-icon small class="mp-2" @click="editItem(item)">edit</v-icon>
          <v-icon small @click="deleteItem(item)">delete</v-icon>
        </template>
        <template v-slot:item.condicion="{ item }">
          <v-card-text v-if="item.condicion" class="blue--text">Activo</v-card-text>
          <v-card-text v-if="!item.condicion" class="red--text">Inactivo</v-card-text>
        </template>
        <template v-slot:no-data>
          <v-btn color="primary" @click="initialize">Reset</v-btn>
        </template>
      </v-data-table>
    </v-flex>
  </v-layout>
</template>
<script>
import axios from "axios";
export default {
  data() {
    return {
      categorias: [],
      //pegar el primer codigo
      dialog: false,
      headers: [
        {
          text: "Opciones",value: "opciones", sortable: false,
        },
        { text: "Nombre", value: "nombre" },
        { text: "Descripcion", value: "descripcion", sortable: false }, //Sortable:false es para que no se ordene
        { text: "Estado", value: "condicion", sortable: false }
      ],
      search: "",
      editedIndex: -1,
      editedItem: {
        name: "",
        calories: 0,
        fat: 0,
        carbs: 0,
        protein: 0
      },
      id: "",
      nombre: "",
      descripcion: ""
    };
  },
  computed: {
    formTitle() {
      return this.editedIndex === -1
        ? "Nueva categoría"
        : "Actualizar categoría";
    }
  },

  watch: {
    dialog(val) {
      val || this.close();
    }
  },

  created() {
    this.listar();
  },
  //fin
  methods: {
    // 1er metodo para traer informacion
    listar() {
      //Traer en el datatable
      let me = this;
      axios
        .get("/api/Categorias/Listar")
        .then(function(response) {
          //CODIGO
          // console.log(response);
          me.categorias = response.data;
        })
        .catch(function(error) {
          console.log(error);
        });
    },

    editItem(item) {
      this.editedIndex = this.desserts.indexOf(item);
      this.editedItem = Object.assign({}, item);
      this.dialog = true;
    },

    deleteItem(item) {
      const index = this.desserts.indexOf(item);
      confirm("Are you sure you want to delete this item?") &&
        this.desserts.splice(index, 1);
    },
    close() {
      this.dialog = false;
    },
    limpiar() {
      this.id = "";
      this.nombre = "";
      this.descripcion = "";
    },

    guardar() {
      if (this.editedIndex > -1) {
      } else {
        let me = this;
        axios
          .post("api/Categorias/CrearCategoria", {
            nombre: me.nombre,
            descripcion: me.descripcion
          })
          .then(function(response) {
            me.close();
            me.listar();
            me.limpiar();
          })
          .catch(function(response) {
            console.log(error);
          });
      }
      //this.close();
    }
  }
};
</script>