import { Link } from "react-router-dom";

function Header() {
  return (
    <header className="fixed top-0 z-10 px-4 py-3">
      <div className="flex flex-col items-center space-y-3">
        {/* Logo Container */}
        <div className="flex text-white items-center p-4 justify-center bg-[#4445] rounded-full backdrop-blur-sm">
          <Link to="/">
            <h1>
              <img width={32} className="h-auto" src="/images/logo.svg" alt="MS" />
            </h1>
          </Link>
        </div>
        
        {/* System Design Button */}
        <Link 
          to="/system-design" 
          className="bg-blue-600 hover:bg-blue-700 text-white px-3 py-1 rounded-lg text-xs font-medium transition-colors shadow-lg"
        >
          System Design
        </Link>
      </div>
    </header>
  );
}

export default Header;
