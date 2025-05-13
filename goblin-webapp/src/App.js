import React, { useState, useEffect } from 'react';
import './App.css';

const API_BASE_URL = process.env.REACT_APP_API_BASE_URL || ''; // Defaults to relative paths

// Placeholder for ProductList component
const ProductList = ({ products, onEdit, onDelete, onOpenProduct }) => {
  if (!products || products.length === 0) {
    return <p>No products yet. Add some!</p>;
  }
  return (
    <table>
      <thead>
        <tr>
          <th>Name</th>
          <th>EAN</th>
          <th>Storage Location</th>
          <th>Expiry Date</th>
          <th>Opened At</th>
          <th>Shelf Life After Opening (Days)</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        {products.map(product => (
          <tr key={product.id}>
            <td>{product.name}</td>
            <td>{product.ean || '-'}</td>
            <td>{product.storageLocation || '-'}</td>
            <td>{product.expiryDate ? new Date(product.expiryDate).toLocaleDateString('en-GB') : '-'}</td>
            <td>{product.openedAt ? new Date(product.openedAt).toLocaleDateString('en-GB') : '-'}</td >
            <td>{product.shelfLifeAfterOpening || '-'}</td>
            <td>
              <button onClick={() => onOpenProduct(product.id)} disabled={!!product.openedAt}>Open</button>
              <button onClick={() => onEdit(product)}>Edit</button>
              <button onClick={() => onDelete(product.id)}>Delete</button>
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  );
};

// New component for displaying products expiring soon
const ExpiringSoonPanel = ({ products }) => {
  if (!products || products.length === 0) {
    return (
      <div>
        <h3>Expiring Soon (Next 14 Days)</h3>
        <p>No products expiring soon.</p>
      </div>
    );
  }
  return (
    <div>
      <h3>Expiring Soon (Next 14 Days)</h3>
      <table>
        <thead>
          <tr>
            <th>Name</th>
            <th>Expiry Date</th>
            <th>Storage Location</th>
          </tr>
        </thead>
        <tbody>
          {products.map(product => (
            <tr key={product.id}>
              <td>{product.name}</td>
              <td>{new Date(product.expiryDate).toLocaleDateString('en-GB')}</td>
              <td>{product.storageLocation || '-'}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

// Placeholder for AddProductForm component
const AddProductForm = ({ onAddProduct, editingProduct, onUpdateProduct }) => {
  const [name, setName] = useState('');
  const [ean, setEan] = useState('');
  const [storageLocation, setStorageLocation] = useState('');
  const [expiryDate, setExpiryDate] = useState('');
  const [openedAt, setOpenedAt] = useState('');
  const [shelfLifeAfterOpening, setShelfLifeAfterOpening] = useState('');

  const isEditing = !!editingProduct;

  useEffect(() => {
    if (editingProduct) {
      setName(editingProduct.name || '');
      setEan(editingProduct.ean || '');
      setStorageLocation(editingProduct.storageLocation || '');
      setExpiryDate(editingProduct.expiryDate ? editingProduct.expiryDate.split('T')[0] : '');
      setOpenedAt(editingProduct.openedAt ? editingProduct.openedAt.split('T')[0] : '');
      setShelfLifeAfterOpening(editingProduct.shelfLifeAfterOpening || '');
    } else {
      // Reset form when not editing
      setName('');
      setEan('');
      setStorageLocation('');
      setExpiryDate('');
      setOpenedAt('');
      setShelfLifeAfterOpening('');
    }
  }, [editingProduct]);

  const handleSubmit = (e) => {
    e.preventDefault();
    const productData = {
      name,
      ean: ean || null,
      storageLocation: storageLocation || null,
      expiryDate: expiryDate || null,
      openedAt: openedAt || null, // Can be null if not opened
      shelfLifeAfterOpening: shelfLifeAfterOpening ? parseInt(shelfLifeAfterOpening) : null,
    };
    if (isEditing) {
        onUpdateProduct(editingProduct.id, productData);
    } else {
        onAddProduct(productData);
    }
    // Reset form fields if not editing (editing form persists for further edits)
    if (!isEditing) {
        setName('');
        setEan('');
        setStorageLocation('');
        setExpiryDate('');
        setOpenedAt('');
        setShelfLifeAfterOpening('');
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <h3>{isEditing ? 'Edit Product' : 'Add New Product'}</h3>
      <div>
        <label>Name: </label>
        <input type="text" value={name} onChange={e => setName(e.target.value)} required />
      </div>
      <div>
        <label>EAN: </label>
        <input type="text" value={ean} onChange={e => setEan(e.target.value)} />
      </div>
      <div>
        <label>Storage Location: </label>
        <input type="text" value={storageLocation} onChange={e => setStorageLocation(e.target.value)} />
      </div>
      <div>
        <label>Expiry Date: </label>
        <input type="date" value={expiryDate} onChange={e => setExpiryDate(e.target.value)} />
      </div>
      <div>
        <label>Opened At: </label>
        <input type="date" value={openedAt} onChange={e => setOpenedAt(e.target.value)} />
      </div>
      <div>
        <label>Shelf Life After Opening (days): </label>
        <input type="number" value={shelfLifeAfterOpening} onChange={e => setShelfLifeAfterOpening(e.target.value)} />
      </div>
      <button type="submit">{isEditing ? 'Update Product' : 'Add Product'}</button>
      {isEditing && <button type="button" onClick={() => onUpdateProduct(null, null)}>Cancel Edit</button>}
    </form>
  );
};

function App() {
  const [products, setProducts] = useState([]);
  const [editingProduct, setEditingProduct] = useState(null); // null or product object
  const [error, setError] = useState(null);

  // Fetch products
  const fetchProducts = async () => {
    try {
      setError(null);
      const response = await fetch(`${API_BASE_URL}/api/products`);

      if (response.status === 304) {
        // Content has not been modified, no need to parse JSON or update state.
        // console.log('Products data not modified (304).');
        return; 
      }

      if (!response.ok) {
        // For other errors (4xx, 5xx), try to get error message from body if possible
        let errorText = `HTTP error! status: ${response.status}`;
        try {
            const errorData = await response.text();
            if (errorData) errorText += ` - ${errorData}`;
        } catch (textError) {
            // Ignore if reading text fails, stick with status code
        }
        throw new Error(errorText);
      }
      const data = await response.json();
      setProducts(data);
    } catch (e) {
      console.error("Failed to fetch products:", e);
      setError(e.message);
    }
  };

  useEffect(() => {
    fetchProducts();
  }, []);

  // Add product
  const handleAddProduct = async (productData) => {
    try {
      setError(null);
      const response = await fetch(`${API_BASE_URL}/api/products`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(productData),
      });
      if (!response.ok) {
        const errorData = await response.text();
        throw new Error(`HTTP error! status: ${response.status} - ${errorData}`);
      }
      fetchProducts(); // Refresh list
      setEditingProduct(null); // Clear editing state
    } catch (e) {
      console.error("Failed to add product:", e);
      setError(e.message);
    }
  };

  // Update product
  const handleUpdateProduct = async (id, productData) => {
    if (!id || !productData) { // Cancelling edit
        setEditingProduct(null);
        return;
    }
    try {
      setError(null);
      const response = await fetch(`${API_BASE_URL}/api/products/${id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(productData),
      });
      if (!response.ok) {
        const errorData = await response.text();
        throw new Error(`HTTP error! status: ${response.status} - ${errorData}`);
      }
      fetchProducts(); // Refresh list
      setEditingProduct(null); // Clear editing state
    } catch (e) {
      console.error("Failed to update product:", e);
      setError(e.message);
    }
  };

  // Delete product
  const handleDeleteProduct = async (id) => {
    if (!window.confirm("Are you sure you want to delete this product?")) return;
    try {
      setError(null);
      const response = await fetch(`${API_BASE_URL}/api/products/${id}`, {
        method: 'DELETE',
      });
      if (!response.ok) {
        const errorData = await response.text();
        throw new Error(`HTTP error! status: ${response.status} - ${errorData}`);
      }
      fetchProducts(); // Refresh list
    } catch (e) {
      console.error("Failed to delete product:", e);
      setError(e.message);
    }
  };
  
  // Open product (Phase 3 - placeholder for now, will need a dedicated endpoint)
  const handleOpenProduct = async (id) => {
    try {
      setError(null);
      const response = await fetch(`${API_BASE_URL}/api/products/${id}/open`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
      });
      if (!response.ok) {
        const errorData = await response.text(); // Or response.json() if backend sends JSON error
        throw new Error(`HTTP error! status: ${response.status} - ${errorData || response.statusText}`);
      }
      fetchProducts(); // Refresh list to show the updated OpenedAt date and disable button
    } catch (e) {
      console.error(`Failed to open product ${id}:`, e);
      setError(e.message);
    }
  };

  const handleEditProduct = (product) => {
    setEditingProduct(product);
  };

  // Calculate products expiring soon
  const currentDate = new Date();
  currentDate.setHours(0, 0, 0, 0); // Normalize to start of day for accurate comparison

  const twoWeeksFromNow = new Date();
  twoWeeksFromNow.setDate(currentDate.getDate() + 14);
  twoWeeksFromNow.setHours(0, 0, 0, 0); // Normalize to start of day

  const expiringSoonProducts = products
    .filter(product => {
      if (!product.expiryDate) return false;
      const expiry = new Date(product.expiryDate);
      expiry.setHours(0,0,0,0); // Normalize product expiry date to start of day for comparison
      return expiry >= currentDate && expiry <= twoWeeksFromNow;
    })
    .sort((a, b) => new Date(a.expiryDate) - new Date(b.expiryDate));

  return (
    <div className="App">
      <header className="App-header">
        <h1>Goblin Market - Household Inventory</h1>
      </header>
      <main>
        {error && <p style={{color: 'red'}}>Error: {error}</p>}
        <ExpiringSoonPanel products={expiringSoonProducts} />
        <hr /> 
        <AddProductForm 
            onAddProduct={handleAddProduct} 
            editingProduct={editingProduct} 
            onUpdateProduct={handleUpdateProduct} 
        />
        <h2>Product List</h2>
        <ProductList 
            products={products} 
            onEdit={handleEditProduct} 
            onDelete={handleDeleteProduct} 
            onOpenProduct={handleOpenProduct} 
        />
      </main>
    </div>
  );
}

export default App;
