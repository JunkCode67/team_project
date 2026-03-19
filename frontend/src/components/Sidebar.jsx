function Sidebar() {
  return (
    // Главный контейнер меню: фиксированная ширина, высота на весь экран, рамка справа
    <div className="w-64 h-screen border-r border-gray-200 bg-white flex flex-col p-4 fixed left-0 top-0">
      
      {/* Логотип */}
      <div className="flex items-center gap-2 mb-8 px-2 mt-2">
        <div className="w-8 h-8 bg-blue-500 rounded-full"></div> {/* Кружок вместо картинки */}
        <span className="text-xl font-bold text-gray-800 tracking-tight">KaggleClone</span>
      </div>

      {/* Кнопка Create (черная, скругленная) */}
      <button className="bg-black text-white rounded-full py-2.5 px-4 flex items-center gap-2 mb-8 hover:bg-gray-800 transition-colors">
        <span className="text-xl leading-none">+</span>
        <span className="font-semibold">Create</span>
      </button>

      {/* Навигация (ссылки) */}
      <nav className="flex flex-col gap-1">
        <a href="#" className="flex items-center gap-3 px-3 py-2.5 rounded-xl text-gray-700 hover:bg-gray-100 transition-colors">
          <span className="font-medium">🏠 Home</span>
        </a>
        
        {/* Активная вкладка (голубой фон) */}
        <a href="#" className="flex items-center gap-3 px-3 py-2.5 rounded-xl bg-blue-50 text-blue-700 font-semibold transition-colors">
          <span>🏆 Competitions</span>
        </a>
        
        <a href="#" className="flex items-center gap-3 px-3 py-2.5 rounded-xl text-gray-700 hover:bg-gray-100 transition-colors">
          <span className="font-medium">📊 Datasets</span>
        </a>
        
        <a href="#" className="flex items-center gap-3 px-3 py-2.5 rounded-xl text-gray-700 hover:bg-gray-100 transition-colors">
          <span className="font-medium">💻 Code</span>
        </a>
      </nav>
    </div>
  );
}

export default Sidebar;