import { useEffect, useState } from "react";
import './OfficeLocation.css'

const OfficeLocation = () => {
  const [offices, setOffices] = useState([]);
  const [skip, setSkip] = useState(0);
  const [isExpanded, setIsExpanded] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const take = 4;
  const totalCount = 10;

  const loadMore = async () => {
    setLoading(true);
    setError(null);

    try {
      const res = await fetch(`http://localhost:5298/AFHLocations?skip=${skip}&take=${take}`);

      if (!res.ok) {
        throw new Error(`Server error: ${res.status} ${res.statusText}`);
      }

      const data = await res.json();
      console.log(data);

      setOffices(prev => [...prev, ...data]);
      setSkip(prev => prev + take);

      if (offices.length + data.length >= totalCount) {
        setIsExpanded(true);
      }
    } catch (err) {
      console.error("Failed to load locations:", err);
      setError("Failed to load office locations. Please try again.");
    } finally {
      setLoading(false);
    }
  };

  const showLess = () => {
    setOffices(offices.slice(0, take));
    setSkip(take);
    setIsExpanded(false);
  };

  useEffect(() => {
    loadMore();
  }, []);

  return (
    <div className="office-container">
      <h1>AFH Office Locations</h1>

      {error && <p className="error-message">{error}</p>}

      {offices.map((office, i) => (
        <div key={i} className="office-card">
          <h3>{office.name}</h3>
          <p>{office.address1}, {office.address2}, {office.city}, {office.postCode}</p>
        </div>
      ))}

      {loading && <p>Loading...</p>}

      {!loading && !isExpanded && offices.length < totalCount && (
        <button className="btn" onClick={loadMore}>Load More</button>
      )}

      {isExpanded && (
        <button className="btn" onClick={showLess}>Show Less</button>
      )}
    </div>
  );
};

export default OfficeLocation;
