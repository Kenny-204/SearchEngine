function Header() {
  return (
    <header className="fixed w-[100px] top-0 z-10 px-4 py-3">
      <div className="flex text-white items-center p-4 justify-center bg-[#4445] rounded-full backdrop-blur-sm">
        <div>
          <h1>
            <img width={32} className="h-auto" src="/images/logo.svg" alt="MS" />
          </h1>
        </div>
        {/* <div className="flex-1 flex justify-center">
          <nav>
            <ul className="inline-flex items-center">
              <li className="p-4">Find</li>
              <li className="p-4">Upload</li>
            </ul>
          </nav>
        </div> */}
      </div>
    </header>
  );
}

export default Header;
