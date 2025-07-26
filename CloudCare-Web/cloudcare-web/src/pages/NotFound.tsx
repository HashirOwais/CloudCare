import { Link } from "react-router-dom"

const NotFound = () => {
  return (
    <div>
      <h1>404</h1>
      <p>Page not found</p>

      <button className="bg-blue-500 text-white p-2 rounded-md">
        <Link to="/">Go to home</Link>
      </button>
    </div>
  )
}

export default NotFound