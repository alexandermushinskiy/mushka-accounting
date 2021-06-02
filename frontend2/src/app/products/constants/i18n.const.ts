export const I18N = {
  title: 'products.title',
  dialogs: {
    deleteProduct: {
      title: 'Удалить товар',
      message: 'products.areYouSureToDeleteProduct',
      confirmLabel: 'button.remove',
      cancelLabel: 'Отмена'
    },
    deleteCategory: {
      title: 'Удалить товар',
      message: 'products.areYouSureToDeleteCategory',
      confirmLabel: 'button.remove',
      cancelLabel: 'Отмена'
    }
  },
  messages: {
    categoryWasAdded: 'products.categoryWasAdded',
    categoryWasUpdated: 'products.categoryWasUpdated',
    categoryDeleted: 'products.categoryWasDeleted',
    productAdded: 'products.messages.productAdded',
    productUpdated: 'products.messages.productUpdated',
    productDeleted: 'products.messages.productDeleted'
  },
  errors: {
    fetchProductsError: 'Ошибка при загрузке товаров.',
    fetchCategoriesError: 'Ошибка при загрузке категорий.',
    saveCategoryError: 'products.saveCategoryError',
    deleteCategoryError: 'products.deleteCategoryError'
  }
};
