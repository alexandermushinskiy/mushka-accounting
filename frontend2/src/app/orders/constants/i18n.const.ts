export const I18N = {
  actionsBar: {
    title: 'orders.orders',
    total: 'common.total',
    actions: {
      addOrder: 'button.addOrder',
      exportToCsv: 'common.exportToCsv'
    }
  },
  table: {
    emptyMessage: 'datatable.emptyMessage',
    columns: {
      orderDate: 'datatable.orderDate',
      orderNumber: 'datatable.orderNumber',
      productsCount: 'datatable.productsCount',
      customerName: 'datatable.customerName',
      deliveryAddress: 'datatable.deliveryAddress'
    }
  },
  remove: {
    buttonHint: 'button.remove',
    confirmMessage: 'orders.areYouSureToDeleteOrder',
    succeessMessage: 'orders.orderDeletedSuccessfully'
  },
  create: {
    successMessage: 'orders.orderAdded'
  },
  update: {
    successMessage: 'orders.orderUpdated'
  },
  dialogs: {
    deleteOrder: {
      title: 'Удалить заказ',
      message: 'orders.areYouSureToDeleteOrder',
      confirmLabel: 'button.remove',
      cancelLabel: 'Отмена'
    }
  },
  errors: {
    searchOrdersError: 'Невозможно загрузить заказы.',
    deleteOrderError: 'Ошибка при удалении заказа.',
    fetchOrderError: 'Ошибка при загрузке заказа.',
    fetchOrderDefaultProductsError: 'Ошибка при загрузке вспомогательных товаров заказа.',
    validateOrderNumberError: 'Ошибка при проверки номера заказа.',
    saveOrderError: 'Ошибка при сохранении заказа.'
  }
};
